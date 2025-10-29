using Ardalis.Result;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.Download;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Models.Download;
using System.Web;

namespace AuditSystem.BusinessLogic.Services.DownloadFile;

public class DecodeDownloadService(IFilePathBuilder filePathBuilder, ISignedUrlService signedUrlService) : IDecodeDownloadService
{
    private readonly IFilePathBuilder _filePathBuilder = filePathBuilder;
    private readonly ISignedUrlService _signedUrlService = signedUrlService;

    public async Task<Result<DownloadFileResult>> DownloadFileAsync(string signedUrl, CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(signedUrl, UriKind.Absolute, out var uri))
            return Result<DownloadFileResult>.Invalid([new ValidationError("Invalid signed URL.")]);

        var query = HttpUtility.ParseQueryString(uri.Query);
        var file = query["file"];
        var expires = query["expires"];
        var signature = query["signature"];

        if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(expires) || string.IsNullOrEmpty(signature))
            return Result<DownloadFileResult>.Invalid([new ValidationError("Missing query parameters.")]);

        var decodedFile = Uri.UnescapeDataString(file);
        var decodedSignature = Uri.UnescapeDataString(signature);

        if (!long.TryParse(expires, out var expiresUnix))
            return Result<DownloadFileResult>.Invalid([new ValidationError("Invalid expires timestamp.")]);

        if (!_signedUrlService.ValidateSignedUrl(decodedFile, expiresUnix, decodedSignature))
            return Result<DownloadFileResult>.Unauthorized("Invalid or expired signature.");

        var fullPath = _filePathBuilder.CombineWithUploadPath(decodedFile);
        if (!File.Exists(fullPath))
            return Result<DownloadFileResult>.NotFound("File not found.");

        // Async file read
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

        var contentType = FileUtilities.GetContentType(fullPath);
        var originalFileName = Path.GetFileName(fullPath);
        var cleanFileName = FileUtilities.GetCleanFileName(originalFileName);

        await Task.Yield();
        return Result<DownloadFileResult>.Success(new DownloadFileResult(stream, contentType, cleanFileName));
    }

}
