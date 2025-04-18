using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Update;

internal sealed class UpdateAuditPlanSummaryReportCommandHandler(
    IAuditPlanSummaryReportService auditPlanSummaryReportService,
    IMapper mapper,
    ILogger<UpdateAuditPlanSummaryReportCommandHandler> logger)
    : IRequestHandler<UpdateAuditPlanSummaryReportCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditPlanSummaryReportCommand request, CancellationToken cancellationToken)
    {
        var auditPlanSummaryReportModel = mapper.Map<AuditPlanSummaryReportModel>(request);
        await auditPlanSummaryReportService.UpdateAuditPlanSummaryReportAsync(auditPlanSummaryReportModel);

        logger.LogInformation("Audit plan summary report updated successfully");
        return Result.Success();
    }
}