using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Get;

internal sealed class GetComplianceChecklistByIdQueryHandler(
    IComplianceChecklistService complianceChecklistService) : 
    IRequestHandler<GetComplianceChecklistByIdQuery, Result<GetComplianceChecklistByIdQueryResponse>>
{
    public async Task<Result<GetComplianceChecklistByIdQueryResponse>> Handle(GetComplianceChecklistByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await complianceChecklistService.GetComplianceChecklistByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetComplianceChecklistByIdQueryResponse>.Success(new GetComplianceChecklistByIdQueryResponse(result));
    }
}