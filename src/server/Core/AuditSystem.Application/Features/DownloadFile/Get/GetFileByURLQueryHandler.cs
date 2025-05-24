using Ardalis.Result;
using AuditSystem.Contract.Interfaces.Download;
using AuditSystem.Contract.Interfaces.FileStorage;
using MediatR;

namespace AuditSystem.Application.Features.DownloadFile.Get;



public sealed class GetFileByURLQueryHandler
    : IRequestHandler<GetFileByURLQuery, Result<GetFileByURLQueryResponse>>
{
    private readonly IDecodeDownloadService _decodeDownloadService;
    private readonly IFilePathBuilder _filePathBuilder;

    public GetFileByURLQueryHandler(
        IDecodeDownloadService decodeDownloadService,
        IFilePathBuilder filePathBuilder)
    {
        _decodeDownloadService = decodeDownloadService;
        _filePathBuilder = filePathBuilder;
    }

    public async Task<Result<GetFileByURLQueryResponse>> Handle(
        GetFileByURLQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _decodeDownloadService.DownloadFileAsync(request.URL, cancellationToken);

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

