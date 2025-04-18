using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Delete;
internal sealed class DeleteAuditPlanSummaryCommandHandler(
    IAuditPlanSummaryReportService auditPlanSummaryReportService,
    ILogger<DeleteAuditPlanSummaryCommandHandler> logger) :
    IRequestHandler<DeleteAuditPlanSummaryCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditPlanSummaryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditPlanSummaryReportService.DeleteAuditPlanSummaryReportAsync(request.Id);
            logger.LogInformation("Audit Plan Summary Report deleted successfully");
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting Audit Plan Summary Report");
            return Result.Error(ex.Message);
        }
    }
}
