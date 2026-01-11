using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.Security;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
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

    private const long MaxFileSizeBytes = 200 * 1024 * 1024;
    private static readonly TimeSpan ScanTimeout = TimeSpan.FromSeconds(30);

    private static readonly string[] AllowedExtensions =
    [
        ".pdf",
        ".docx", ".doc", ".docm", ".dotx", ".dotm", ".rtf",
        ".xlsx", ".xls", ".xlsm", ".xltx", ".xltm",
        ".pptx", ".ppt", ".ppsm", ".ppsx", ".potx", ".potm"
    ];

    private static readonly Dictionary<string, byte[][]> MagicBytes = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".pdf",  [[0x25, 0x50, 0x44, 0x46]] },
        { ".rtf",  [[0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31]] },
        { ".doc",  [[0xD0,0xCF,0x11,0xE0,0xA1,0xB1,0x1A,0xE1]] },
        { ".xls",  [[0xD0,0xCF,0x11,0xE0,0xA1,0xB1,0x1A,0xE1]] },
        { ".ppt",  [[0xD0,0xCF,0x11,0xE0,0xA1,0xB1,0x1A,0xE1]] },

        { ".docx", [[0x50,0x4B,0x03,0x04]] },
        { ".xlsx", [[0x50,0x4B,0x03,0x04]] },
        { ".pptx", [[0x50,0x4B,0x03,0x04]] },
        { ".docm", [[0x50,0x4B,0x03,0x04]] },
        { ".xlsm", [[0x50,0x4B,0x03,0x04]] }
    };

    private static readonly Dictionary<string, string[]> ExpectedZipEntries = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".docx", ["[Content_Types].xml", "word/document.xml"] },
        { ".xlsx", ["[Content_Types].xml", "xl/workbook.xml"] },
        { ".pptx", ["[Content_Types].xml", "ppt/presentation.xml"] }
    };

    public async Task<Result<FileSaveDetails>> SaveFileAsync(
        IFormFile file,
        Guid userId,
        string serviceName,
        string userProvidedFileName)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result.Error("No file uploaded.");

            if (file.Length > MaxFileSizeBytes)
                return Result.Error("File too large.");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                return Result.Error("Invalid file type.");

            var sanitized = SanitizeFileName(userProvidedFileName);
            var uniqueFileName = $"{sanitized}_{Guid.NewGuid():N}{ext}";
            var fullPath = _pathBuilder.GetFullPath(userId, serviceName, uniqueFileName);

            // ---- MAGIC BYTES
            if (!ValidateMagicBytes(file, ext))
                return Result.Error("File content does not match its extension.");

            // ---- ZIP STRUCTURE VALIDATION
            if (IsOpenXml(ext) && !ValidateOfficeZipStructure(file, ext))
                return Result.Error("Invalid or corrupted Office document.");

            // ---- MACRO ENFORCEMENT
            if (!IsMacroAllowed(file, ext))
                return Result.Error("Macros detected in non-macro-enabled document.");

            // ---- MALWARE SCAN
            using var cts = new CancellationTokenSource(ScanTimeout);
            try
            {
                await using var scanStream = file.OpenReadStream();
                var scan = await _malwareScanner.ScanStreamAsync(scanStream, file.FileName, cts.Token);
                if (!scan.IsSuccess)
                    return Result.Error("Malware detected.");
            }
            catch (OperationCanceledException)
            {
                return Result.Error("File scanning service timed out.");
            }

            // ---- WRITE TO DISK
            await using (var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }

            return Result.Success(new FileSaveDetails
            {
                SavedFileName = uniqueFileName,
                FullPath = fullPath,
                FileSize = file.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File save failed");
            return Result.Error("Unexpected error.");
        }
    }

    // ---------------- HELPERS ----------------

    private static bool ValidateMagicBytes(IFormFile file, string ext)
    {
        if (!MagicBytes.TryGetValue(ext, out var expected))
            return false;

        using var stream = file.OpenReadStream();
        var maxLen = expected.Max(x => x.Length);
        var buffer = new byte[maxLen];

        if (stream.Read(buffer, 0, maxLen) < maxLen)
            return false;

        return expected.Any(sig =>
            buffer.Take(sig.Length).SequenceEqual(sig));
    }

    private static bool ValidateOfficeZipStructure(IFormFile file, string ext)
    {
        if (!ExpectedZipEntries.TryGetValue(ext, out var expected))
            return true;

        using var stream = file.OpenReadStream();
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);

        var entries = zip.Entries.Select(e => e.FullName).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return expected.All(entries.Contains);
    }

    private static bool IsMacroAllowed(IFormFile file, string ext)
    {
        if (!IsOpenXml(ext))
            return true;

        using var stream = file.OpenReadStream();
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read);

        var hasMacro = zip.Entries.Any(e =>
            e.FullName.EndsWith("vbaProject.bin", StringComparison.OrdinalIgnoreCase));

        return ext switch
        {
            ".docm" or ".xlsm" or ".pptm" => true,
            _ => !hasMacro
        };
    }

    private static bool IsOpenXml(string ext) =>
        ext.EndsWith("x") || ext.EndsWith("m");

    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "file";

        // Handle both / and \ as separators for sanitization across platforms
        var baseName = Path.GetFileNameWithoutExtension(name.Replace('\\', '/'));
        
        if (string.IsNullOrWhiteSpace(baseName)) return "file";

        var clean = Regex.Replace(baseName, @"[^\w\-.]", "_");
        
        // If sanitized name is empty or contains only dots and underscores, fallback to 'file'
        if (string.IsNullOrWhiteSpace(clean) || clean.All(c => c == '.' || c == '_'))
            return "file";

        return clean.Length > 200 ? clean[..200] : clean;
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
            _logger.LogError(ex, "Delete failed");
            return Task.FromResult(Result.Error("Delete failed."));
        }
    }
}