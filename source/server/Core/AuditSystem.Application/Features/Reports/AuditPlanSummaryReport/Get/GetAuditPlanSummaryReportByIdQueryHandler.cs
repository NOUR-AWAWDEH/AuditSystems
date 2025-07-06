using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Get;

internal sealed class GetAuditPlanSummaryReportByIdQueryHandler(
    IAuditPlanSummaryReportService auditPlanSummaryReportService) :
    IRequestHandler<GetAuditPlanSummaryReportByIdQuery, Result<GetAuditPlanSummaryReportByIdQueryResponse>>
{
    public async Task<Result<GetAuditPlanSummaryReportByIdQueryResponse>> Handle(GetAuditPlanSummaryReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditPlanSummaryReportService.GetAuditPlanSummaryReportByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditPlanSummaryReportByIdQueryResponse>.Success(new GetAuditPlanSummaryReportByIdQueryResponse(result));
    }
}
