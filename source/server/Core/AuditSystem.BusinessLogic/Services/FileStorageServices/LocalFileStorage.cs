using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.Security;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class LocalFileStorage( 
        IFilePathBuilder pathBuilder,
        IMalwareScanner malwareScanner,
        ILogger<LocalFileStorage> logger) 
        : IFileStorage
{
    private readonly IFilePathBuilder _pathBuilder = pathBuilder;
    private readonly IMalwareScanner _malwareScanner = malwareScanner;
    private readonly ILogger<LocalFileStorage> _logger = logger;

    private static readonly string[] AllowedExtensions =
    [ 
        // PDF
        ".pdf",
        // Word formats
        ".docx", ".doc", ".docm", ".dotx", ".dotm", ".rtf",
        // Excel formats
        ".xlsx", ".xls", ".xlsm", ".xltx", ".xltm",
        // PowerPoint formats
        ".pptx", ".ppt", ".ppsm", ".ppsx", ".potx", ".potm"
    ];

    private static readonly Dictionary<string, byte[][]> MagicBytes = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".pdf", [ [0x25, 0x50, 0x44, 0x46] ] }, // %PDF
        { ".docx", [ [0x50, 0x4B, 0x03, 0x04] ] }, // PK..
        { ".xlsx", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".pptx", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".docm", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".xlsm", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".ppsm", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".dotx", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".xltx", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".potx", [ [0x50, 0x4B, 0x03, 0x04] ] },
        { ".doc", [ [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1] ] }, // Legacy Office
        { ".xls", [ [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1] ] },
        { ".ppt", [ [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1] ] },
        { ".rtf", [ [0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31] ] } // {\rtf1
    };

    private const long MaxFileSizeBytes = 200 * 1024 * 1024;
    private static readonly TimeSpan ScanTimeout = TimeSpan.FromSeconds(30);

    public async Task<Result<FileSaveDetails>> SaveFileAsync(IFormFile file, Guid userId, string serviceName, string userProvidedFileName)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result.Error("No file uploaded.");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                return Result.Error($"Invalid file type. Allowed: {string.Join(", ", AllowedExtensions)}");

            if (file.Length > MaxFileSizeBytes)
                return Result.Error("File too large (200MB max).");


            var sanitized = SanitizeFileName(userProvidedFileName);
            var uniqueFileName = $"{sanitized}_{Guid.NewGuid():N}{ext}";

            var fullPath = _pathBuilder.GetFullPath(userId, serviceName, uniqueFileName);
            
            // Validate Magic Bytes
            if (MagicBytes.TryGetValue(ext, out var expectedBytes))
            {
                await using var stream = file.OpenReadStream();
                if (!ValidateMagicBytes(stream, expectedBytes))
                {
                    _logger.LogWarning("Magic byte mismatch for file {FileName}. Extension: {Extension}", file.FileName, ext);
                    return Result.Error("File content does not match its extension.");
                }
            }

            // Scan for malware before writing to disk
            using var cts = new CancellationTokenSource(ScanTimeout);
            try
            {
                await using var scanStream = file.OpenReadStream();
                var scanResult = await _malwareScanner.ScanStreamAsync(scanStream, file.FileName, cts.Token);
                if (!scanResult.IsSuccess)
                {
                    _logger.LogWarning("Malware detected or scanning failed for file {FileName}: {Error}", file.FileName, scanResult.Errors.FirstOrDefault());
                    return Result.Error("Malware detected or file content is unsafe.");
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Malware scan timed out for file {FileName}.", file.FileName);
                return Result.Error("Malware scanning service timed out.");
            }
            
            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);

            return Result.Success(new FileSaveDetails
            {
                SavedFileName = uniqueFileName,
                FullPath = fullPath,
                FileSize = file.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Save failed for file: {FileName}", userProvidedFileName);
            return Result.Error("Unexpected error during file save.");
        }
    }

    private static string SanitizeFileName(string name)
    {
        // Handle cross-platform path separators
        var normalizedName = name.Replace('\\', '/');
        var baseName = Path.GetFileNameWithoutExtension(normalizedName);
        if (string.IsNullOrWhiteSpace(baseName)) baseName = "file";
        
        var clean = Regex.Replace(baseName, @"[^\w\-.]", "_").Trim('_', '.', '-');
        
        // Truncate to avoid filesystem limits (255 is common max)
        if (clean.Length > 200) // Leave room for GUID and extension
            clean = clean.Substring(0, 200);
            
        return string.IsNullOrWhiteSpace(clean) ? "file" : clean;
    }

    private static bool ValidateMagicBytes(Stream stream, byte[][] expectedMagicBytes)
    {
        var maxLen = expectedMagicBytes.Max(x => x.Length);
        var buffer = new byte[maxLen];
        var read = stream.Read(buffer, 0, maxLen);
        
        if (read < maxLen) return false;

        return expectedMagicBytes.Any(magic => buffer.Take(magic.Length).SequenceEqual(magic));
    }

    public Task<Result> DeleteFileAsync(string fullFilePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fullFilePath))
                return Task.FromResult(Result.Error("Invalid file path."));
    
            if (!File.Exists(fullFilePath))
            return Task.FromResult(Result.Error("File does not exist."));

            File.Delete(fullFilePath);
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete failed for: {FullFilePath}", fullFilePath);
            return Task.FromResult(Result.Error("Error deleting file."));
        }
    }
}
