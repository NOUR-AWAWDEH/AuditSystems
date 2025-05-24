using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.SupportingDocs;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;

internal class GetSupportingDocURLQueryByIdHandler(
    ISupportingDocService supportingDocService,
    IURLService URLService)
    : IRequestHandler<GetSupportingDocURLQueryById, Result<GetSupportingDocURLQueryByIdResponse>>
{
    private readonly ISupportingDocService _supportingDocService = supportingDocService;
    private readonly IURLService _URLService = URLService;

    public async Task<Result<GetSupportingDocURLQueryByIdResponse>> Handle(GetSupportingDocURLQueryById request, CancellationToken cancellationToken)
    {
        var supportingDoc = await _supportingDocService.GetSupportingDocByIdAsync(request.Id);
        if (supportingDoc is null)
        {
            return Result.NotFound("Document not found.");
        }

        var result = await _URLService.GenerateSignedURLAsync(supportingDoc.UserId, supportingDoc.FileName, "SupportingDocs");
        if (!result.IsSuccess)
        {
            return Result.Error(new ErrorList(result.Errors));
        }

        var response = new GetSupportingDocURLQueryByIdResponse
        {
            FileName = supportingDoc.FileName,
            FileHttpResult = new FileHttpResult(result.Value)
        };

        return Result.Success(response);
    }
}