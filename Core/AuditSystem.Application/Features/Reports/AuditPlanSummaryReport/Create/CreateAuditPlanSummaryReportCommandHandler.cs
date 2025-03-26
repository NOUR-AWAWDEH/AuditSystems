using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;

internal sealed class CreateAuditPlanSummaryReportCommandHandler(
    IAuditPlanSummaryReportService auditPlanSummaryReportService,
    IMapper mapper,
    ILogger<CreateAuditPlanSummaryReportCommandHandler> logger)
    : IRequestHandler<CreateAuditPlanSummaryReportCommand, Result<CreateAuditPlanSummaryReportCommandResponse>>
{
    public async Task<Result<CreateAuditPlanSummaryReportCommandResponse>> Handle(CreateAuditPlanSummaryReportCommand request, CancellationToken cancellationToken)
    {
        var auditPlanSummaryReportModel = mapper.Map<AuditPlanSummaryReportModel>(request);
        var auditPlanSummaryReportId = await auditPlanSummaryReportService.CreateAuditPlanSummaryReportAsync(auditPlanSummaryReportModel);
        logger.LogInformation("Audit Plan Summary Report with Name {AuditPlanSummaryReportName} has been created.", request.ReportName);

        return Result<CreateAuditPlanSummaryReportCommandResponse>.Created(new CreateAuditPlanSummaryReportCommandResponse(auditPlanSummaryReportId));
    }
}
