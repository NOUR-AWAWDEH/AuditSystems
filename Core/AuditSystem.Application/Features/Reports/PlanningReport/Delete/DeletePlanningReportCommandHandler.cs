using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Delete;

internal sealed class DeletePlanningReportCommandHandler(
    IPlanningReportService planningReportService,
    ILogger<DeletePlanningReportCommandHandler> logger) :
    IRequestHandler<DeletePlanningReportCommand, Result>
{
    public async Task<Result> Handle(DeletePlanningReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await planningReportService.DeletePlanningReportAsync(request.Id);
            logger.LogInformation("Planning report with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting planning report with id: {Id}", request.Id);
            return Result.Error(ex.Message); 
        }  
    }
}
