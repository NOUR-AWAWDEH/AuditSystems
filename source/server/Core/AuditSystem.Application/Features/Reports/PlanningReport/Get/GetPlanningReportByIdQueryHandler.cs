using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Get;

internal sealed class GetPlanningReportByIdQueryHandler(
    IPlanningReportService planningReportService) : 
    IRequestHandler<GetPlanningReportByIdQuery, Result<GetPlanningReportByIdQueryResponse>>
{
    public async Task<Result<GetPlanningReportByIdQueryResponse>> Handle(GetPlanningReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await planningReportService.GetPlanningReportByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetPlanningReportByIdQueryResponse>.Success(new GetPlanningReportByIdQueryResponse(result));
    }
}
