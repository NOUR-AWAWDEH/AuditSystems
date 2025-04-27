using Microsoft.Extensions.Logging;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using MediatR;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Delete;

internal sealed class DeleteInternalAuditConsolidationReportCommandHandler(
    IInternalAuditConsolidationReportService internalAuditConsolidationReportService,
    ILogger<DeleteInternalAuditConsolidationReportCommandHandler> logger) : 
    IRequestHandler<DeleteInternalAuditConsolidationReportCommand, Result>
{
    public async Task<Result> Handle(DeleteInternalAuditConsolidationReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await internalAuditConsolidationReportService.DeleteInternalAuditConsolidationReportAsync(request.Id);
            logger.LogInformation("Internal audit consolidation report with id: {Id} was deleted", request.Id);
            return Result.SuccessWithMessage("Internal audit consolidation report was deleted");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting internal audit consolidation report with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}