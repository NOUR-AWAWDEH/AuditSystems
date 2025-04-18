using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Delete;

internal sealed class DeleteReportingFollowUpCommandHandler(
    IReportingFollowUpService reportingFollowUpService,
    ILogger<DeleteReportingFollowUpCommandHandler> logger) :
    IRequestHandler<DeleteReportingFollowUpCommand, Result>
{
    public async Task<Result> Handle(DeleteReportingFollowUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await reportingFollowUpService.DeleteReportingFollowUpAsync(request.Id);
            logger.LogInformation("Reporting follow up with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting reporting follow up with id: {Id}", request.Id);
            return Result.Error("An error occurred while deleting reporting follow up");
        }  
    }
}
