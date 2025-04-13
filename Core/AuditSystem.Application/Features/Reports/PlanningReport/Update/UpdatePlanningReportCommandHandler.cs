using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Update;

internal sealed class UpdatePlanningReportCommandHandler(
    IPlanningReportService planningReportService,
    IMapper mapper,
    ILogger<UpdatePlanningReportCommandHandler> logger)
    : IRequestHandler<UpdatePlanningReportCommand, Result>
{
    public async Task<Result> Handle(UpdatePlanningReportCommand request, CancellationToken cancellationToken)
    {
        var planningReportModel = mapper.Map<PlanningReportModel>(request);
        await planningReportService.UpdatePlanningReportAsync(planningReportModel);

        logger.LogInformation("Planning report updated successfully");
        return Result.Success();
    }
}
