using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class URLService(
        ISignedUrlService signedUrlService,
        ILogger<URLService> logger) 
        : IURLService
{
    private readonly ISignedUrlService _signedUrlService= signedUrlService;
    private readonly ILogger<URLService> _logger = logger;

    public Task<Result<string>> GenerateSignedURLAsync(Guid userId, string fileName, string serviceName)
    {
        try
        {
            var relativePath = Path.Combine(userId.ToString(), serviceName, fileName);
            var signedUrl = _signedUrlService.GenerateSignedUrl(relativePath, TimeSpan.FromMinutes(10));
            return Task.FromResult(Result<string>.Success(signedUrl));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating signed download link for file {FileName}", fileName);
            return Task.FromResult(Result<string>.Error("An error occurred while generating the download link."));
        }
    }

}
