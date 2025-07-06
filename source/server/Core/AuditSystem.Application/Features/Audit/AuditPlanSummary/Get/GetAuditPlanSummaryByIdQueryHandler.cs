using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Get;

internal sealed class GetAuditPlanSummaryByIdQueryHandler(
    IAuditPlanSummaryService auditPlanSummaryService
) : IRequestHandler<GetAuditPlanSummaryByIdQuery, Result<GetAuditPlanSummaryByIdQueryResponse>>
{
    public async Task<Result<GetAuditPlanSummaryByIdQueryResponse>> Handle(GetAuditPlanSummaryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditPlanSummaryService.GetAuditPlanSummaryByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditPlanSummaryByIdQueryResponse>.Success(new GetAuditPlanSummaryByIdQueryResponse(result));
    }
}
