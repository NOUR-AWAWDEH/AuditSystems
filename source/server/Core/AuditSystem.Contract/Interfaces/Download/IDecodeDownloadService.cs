using Ardalis.Result;
using AuditSystem.Contract.Models.Download;

namespace AuditSystem.Contract.Interfaces.Download;

public interface IDecodeDownloadService
{
    Task<Result<DownloadFileResult>> DownloadFileAsync(string signedUrl, CancellationToken cancellationToken);
}