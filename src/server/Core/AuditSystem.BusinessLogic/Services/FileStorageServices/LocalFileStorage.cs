using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class LocalFileStorage( 
        IFilePathBuilder pathBuilder,
        ILogger<LocalFileStorage> logger) 
        : IFileStorage
{
    private readonly IFilePathBuilder _pathBuilder = pathBuilder;
    private readonly ILogger<LocalFileStorage> _logger = logger;

    private static readonly string[] AllowedExtensions =
        [ ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".doc", ".xlsx" ];
    private const long MaxFileSizeBytes = 200 * 1024 * 1024;

    public async Task<Result<FileSaveDetails>> SaveFileAsync(IFormFile file, string userProvidedFileName, Guid userId,string serviceName)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result.Error("No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return Result.Error($"Invalid file type. Allowed: {string.Join(", ", AllowedExtensions)}");

            if (file.Length > MaxFileSizeBytes)
                return Result.Error("File too large (200MB max).");


            var sanitized = SanitizeFileName(userProvidedFileName);
            var unique = $"{sanitized}_{Guid.NewGuid():N}{ext}";

            var fullPath = _pathBuilder.GetFullPath(userId, unique, serviceName);
            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs);

            var relPath = _pathBuilder.GetFullPath(userId, unique, serviceName);

            return Result.Success(new FileSaveDetails
            {
                SavedFileName = unique,
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
        var baseName = Path.GetFileNameWithoutExtension(name);
        var clean = Regex.Replace(baseName, @"[^\w\-.]", "_").Trim('_', '.', '-');
        return string.IsNullOrWhiteSpace(clean) ? "file" : clean;
    }

    public Task<Result> DeleteFileAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.FromResult(Result.Error("Invalid filename."));

            var fullPath = Path.Combine(Environment.GetEnvironmentVariable("FILESTORAGE_UPLOADPATH")!, filePath);
            if (!File.Exists(fullPath)) return Task.FromResult(Result.NotFound("File not found."));

            File.Delete(fullPath);
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Delete failed for: {FilePath}", filePath);
            return Task.FromResult(Result.Error("Error deleting file."));
        }
    }
}
