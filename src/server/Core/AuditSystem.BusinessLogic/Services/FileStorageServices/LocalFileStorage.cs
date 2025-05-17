using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class LocalFileStorage : IFileStorage
{
    private readonly string _uploadFolderPath;
    private readonly string _publicBaseUrl;
    private readonly ILogger<LocalFileStorage> _logger;
    private readonly IWebHostEnvironment _env;

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
            // Validate file
            if (file == null || file.Length == 0)
                return Result.Error("No file uploaded.");

            // Validate file type
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".doc", ".xlsx" };
            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext)) ext = ".dat";
            if (!validExtensions.Contains(ext))
                return Result.Error($"Invalid file type. Allowed: {string.Join(", ", validExtensions)}");

            // Validate file size (50MB max)
            if (file.Length > 50 * 1024 * 1024)
                return Result.Error("File size exceeds 50MB limit.");

            // Generate safe filename
            string sanitizedFileName = Path.GetFileNameWithoutExtension(userProvidedFileName)
                .Replace(" ", "_")
                .Replace("..", "")
                .Trim();
            string uniqueFileName = $"{sanitizedFileName}_{Guid.NewGuid()}{ext}";
            string fullPath = Path.Combine(_uploadFolderPath, uniqueFileName);

            // Save file
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Generate public URL
            string publicUrl = $"{_publicBaseUrl.TrimEnd('/')}/{uniqueFileName}";

            var details = new FileSaveDetails
            {
                SavedFileName = uniqueFileName,
                FullPath = fullPath,
                PublicUrl = publicUrl,
                FileSize = file.Length
            };

            _logger.LogInformation("File saved successfully. Path: {Path}, URL: {Url}",
                fullPath, publicUrl);
            return Result.Success(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file {FileName}", userProvidedFileName);
            return Result.Error("An error occurred while saving the file.");
        }
    }

    public async Task<Result> DeleteFileAsync(string fileName)
    {
        //try
        //{
        //    if (string.IsNullOrWhiteSpace(fileName))
        //        return Result.Error("Invalid filename");

        //    string fullPath = Path.Combine(_uploadFolderPath, fileName);
        //    if (!File.Exists(fullPath))
        //        return Result.NotFound();

        //    File.Delete(fullPath);
        //    return Result.Success();
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error deleting file {FileName}", fileName);
        //    return Result.Error("Failed to delete file");
        //}

        throw new NotImplementedException("DeleteFileAsync is not implemented yet.");
    }
}
