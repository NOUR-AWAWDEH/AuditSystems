using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

namespace AuditSystem.Application.Features.SupportingDocs.Get;

internal sealed class GetSupportingDocsByIdQueryHandler(
    ISupportingDocService supportingDocService) : 
    IRequestHandler<GetSupportingDocsByIdQuery, Result<GetSupportingDocsByIdQueryResponse>>
{
    public async Task<Result<GetSupportingDocsByIdQueryResponse>> Handle(GetSupportingDocsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await supportingDocService.GetSupportingDocByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSupportingDocsByIdQueryResponse>.Success(new GetSupportingDocsByIdQueryResponse(result));
    }
}
