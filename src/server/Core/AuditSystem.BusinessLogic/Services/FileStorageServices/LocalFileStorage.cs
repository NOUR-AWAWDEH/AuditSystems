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

            var fullPath = _pathBuilder.GetFullPath(userId,serviceName, uniqueFileName);

            //var directory = Path.GetDirectoryName(fullPath);
            //if (!string.IsNullOrEmpty(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}
            
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
        var baseName = Path.GetFileNameWithoutExtension(name);
        var clean = Regex.Replace(baseName, @"[^\w\-.]", "_").Trim('_', '.', '-');
        return string.IsNullOrWhiteSpace(clean) ? "file" : clean;
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
