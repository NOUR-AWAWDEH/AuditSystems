using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Delete;

internal sealed class DeleteJobTimeAllocationReportCommandHandler(
    IJobTimeAllocationReportService jobTimeAllocationReportService,
    ILogger<DeleteJobTimeAllocationReportCommandHandler> logger) :
    IRequestHandler<DeleteJobTimeAllocationReportCommand, Result>
{
    public async Task<Result> Handle(DeleteJobTimeAllocationReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await jobTimeAllocationReportService.DeleteJobTimeAllocationReportAsync(request.Id);
            logger.LogInformation("Job time allocation report with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting job time allocation report with id: {Id}", request.Id);
            return Result.Error(ex.Message);  
        }  
    }
}
