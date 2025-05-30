using Ardalis.Result;
using AuditSystem.Contract.Interfaces.Download;
using MediatR;

namespace AuditSystem.Application.Features.DownloadFile.Get;

public sealed class GetFileByURLQueryHandler( IDecodeDownloadService decodeDownloadService)
    : IRequestHandler<GetFileByURLQuery, Result<GetFileByURLQueryResponse>>
{
    public async Task<Result<GetFileByURLQueryResponse>> Handle(
        GetFileByURLQuery request,
        CancellationToken cancellationToken)
    {
        var result = await decodeDownloadService.DownloadFileAsync(request.URL, cancellationToken);

        return result.Status switch
        {
            ResultStatus.Ok => Result<GetFileByURLQueryResponse>.Success(
                new GetFileByURLQueryResponse(
                    result.Value.FileStream,
                    result.Value.ContentType,
                    result.Value.FileName)),
            _ => Result<GetFileByURLQueryResponse>.Error(string.Join("; ", result.Errors))
        };
    }
}