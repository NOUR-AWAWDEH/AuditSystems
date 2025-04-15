using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MediatR;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Delete;

internal sealed class DeleteAuditPlanSummaryReportCommandHandler(
    IAuditPlanSummaryReportService auditPlanSummaryReportService,
    ILogger<DeleteAuditPlanSummaryReportCommandHandler> logger ) : 
    IRequestHandler<DeleteAuditPlanSummaryReportCommand,Result>
{
    public async Task<Result> Handle(DeleteAuditPlanSummaryReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditPlanSummaryReportService.DeleteAuditPlanSummaryReportAsync(request.Id);
            logger.LogInformation("Audit plan summary report with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting audit plan summary report with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}