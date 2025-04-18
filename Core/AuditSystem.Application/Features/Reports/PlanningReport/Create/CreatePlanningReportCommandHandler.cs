using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Create;

internal sealed class CreatePlanningReportCommandHandler(
    IPlanningReportService planningReportService,
    IMapper mapper,
    ILogger<CreatePlanningReportCommandHandler> logger)
    : IRequestHandler<CreatePlanningReportCommand, Result<CreatePlanningReportCommandResponse>>
{
    public async Task<Result<CreatePlanningReportCommandResponse>> Handle(CreatePlanningReportCommand request, CancellationToken cancellationToken)
    {
        var planningReportModel = mapper.Map<PlanningReportModel>(request);
        var planningReportId = await planningReportService.CreatePlanningReportAsync(planningReportModel);
        logger.LogInformation("Planning Report with Name {PlanningReportName} has been created.", request.ReportName);

        return Result<CreatePlanningReportCommandResponse>.Created(new CreatePlanningReportCommandResponse(planningReportId));
    }
}
