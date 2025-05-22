
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.DownloadByFileId;

public class DownloadSupportingDocQueryByIdHandler : IRequestHandler<DownloadSupportingDocQueryById, Result<DownloadSupportingDocQueryByIdResponse>>
{
    private readonly ISupportingDocService _supportingDocService;

    public DownloadSupportingDocQueryByIdHandler(ISupportingDocService supportingDocService)
    {
        _supportingDocService = supportingDocService;
    }

    public async Task<Result<DownloadSupportingDocQueryByIdResponse>> Handle(DownloadSupportingDocQueryById request, CancellationToken cancellationToken)
    {
        //var supportingDoc = await _supportingDocService.GetSupportingDocByIdAsync(request.Id);

        //if (supportingDoc is null)
        //{
        //    return Result.NotFound();
        //}

        //// Assuming the SupportingDocModel contains FileData and FileName properties
        //if (supportingDoc.FileData is null || string.IsNullOrEmpty(supportingDoc.FileName))
        //{
        //    return Result.Error("File data not found for this document.");
        //}

        //return Result.Success(new DownloadSupportingDocQueryByIdResponse
        //{
        //    FileData = supportingDoc.FileData,
        //    FileName = supportingDoc.FileName,
        //    ContentType = supportingDoc.ContentType // Assuming you store content type
        //});

        throw new NotImplementedException("This method is not implemented yet.");
    }
}
