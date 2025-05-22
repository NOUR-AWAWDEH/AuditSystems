using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class LocalFileStorage : IFileStorage
{
    private readonly string _uploadFolderPath;
    private readonly string _publicBaseUrl;
    private readonly ILogger<LocalFileStorage> _logger;
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions =
        { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".doc", ".xlsx" };
    private const long MaxFileSizeBytes = 200 * 1024 * 1024;

    public LocalFileStorage(
        IWebHostEnvironment env,
        ILogger<LocalFileStorage> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _env = env;

        _uploadFolderPath = Environment.GetEnvironmentVariable("FILESTORAGE_UPLOADPATH")
            ?? throw new InvalidOperationException("Missing FILESTORAGE_UPLOADPATH in environment variables or .env file.");

        _publicBaseUrl = Environment.GetEnvironmentVariable("FILESTORAGE_PUBLICBASEURL")
            ?? throw new InvalidOperationException("Missing FILESTORAGE_PUBLICBASEURL in environment variables or .env file.");

        EnsureUploadDirectoryExists();
    }

    private void EnsureUploadDirectoryExists()
    {
        try
        {
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
                _logger.LogInformation("Created upload directory at {Path}", _uploadFolderPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to create upload directory at {Path}", _uploadFolderPath);
            throw;
        }
    }

    public async Task<Result<FileSaveDetails>> SaveFileAsync(IFormFile file, string userProvidedFileName)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result.Error("No file uploaded.");

            // Validate file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
                return Result.Error($"Invalid file type. Allowed types: {string.Join(", ", AllowedExtensions)}");

            // Validate file size
            if (file.Length > MaxFileSizeBytes)
                return Result.Error("File size exceeds the 50MB limit.");

            // Sanitize and generate filename
            var sanitizedFileName = SanitizeFileName(userProvidedFileName);
            var uniqueFileName = $"{sanitizedFileName}_{Guid.NewGuid():N}{extension}";

            // Ensure upload directory exists
            Directory.CreateDirectory(_uploadFolderPath);

            // Create full file path
            var fullPath = Path.Combine(_uploadFolderPath, uniqueFileName);
            fullPath = Path.GetFullPath(fullPath);

            var urlFriendlyFileName = uniqueFileName.Replace('\\', '/');  // Use forward slashes for URLs
            // Save file
            await using var fileStream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // Create URL-friendly path
            var publicUrl = BuildPublicUrl(uniqueFileName);

            var details = new FileSaveDetails
            {
                SavedFileName = uniqueFileName,
                FullPath = fullPath,
                PublicUrl = publicUrl,
                FileSize = file.Length
            };

            _logger.LogInformation("File saved successfully at {FullPath}", fullPath);
            return Result.Success(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file: {FileName}", userProvidedFileName);
            return Result.Error("An unexpected error occurred while saving the file.");
        }
    }

    private string SanitizeFileName(string fileName)
    {
        var baseName = Path.GetFileNameWithoutExtension(fileName);

        // More comprehensive sanitization
        var cleanName = Regex.Replace(baseName, @"[^\w\-.]", "_")
                            .Trim('_', '.', '-')
                            .Trim();

        return string.IsNullOrWhiteSpace(cleanName) ? "file" : cleanName;
    }

    private string BuildPublicUrl(string fileName)
    {
        // Normalize URL components
        var baseUrl = _publicBaseUrl.TrimEnd('/');
        var encodedFileName = Uri.EscapeDataString(fileName.Replace('\\', '/'));

        return $"{baseUrl}/{encodedFileName}";
    }

    public async Task<Result> DeleteFileAsync(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Result.Error("Invalid file name.");

            string fullPath = Path.Combine(_uploadFolderPath, fileName);

            if (!File.Exists(fullPath))
                return Result.NotFound("File not found.");

            File.Delete(fullPath);
            _logger.LogInformation("File deleted: {FullPath}", fullPath);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileName}", fileName);
            return Result.Error("Failed to delete file due to an internal error.");
        }
    }
}
