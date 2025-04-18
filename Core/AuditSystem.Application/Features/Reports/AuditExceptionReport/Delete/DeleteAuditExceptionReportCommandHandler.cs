using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Delete;

internal sealed class DeleteAuditExceptionReportCommandHandler(
    IAuditExceptionReportService auditExceptionReportService,
    ILogger<DeleteAuditExceptionReportCommandHandler> logger) :
    IRequestHandler<DeleteAuditExceptionReportCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditExceptionReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditExceptionReportService.DeleteAuditExceptionReportAsync(request.Id);
            logger.LogInformation("Audit exception report with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting audit exception report with id: {Id}", request.Id);
            return Result.Error("Error while deleting audit exception report");
        }  
    }
}