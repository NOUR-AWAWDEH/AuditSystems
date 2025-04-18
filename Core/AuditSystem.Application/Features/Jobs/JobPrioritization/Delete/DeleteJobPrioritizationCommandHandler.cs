using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Delete;

internal sealed class DeleteJobPrioritizationCommandHandler(
    IJobPrioritizationService jobPrioritizationService,
    ILogger<DeleteJobPrioritizationCommandHandler> logger) :
    IRequestHandler<DeleteJobPrioritizationCommand, Result>
{
    public async Task<Result> Handle(DeleteJobPrioritizationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await jobPrioritizationService.DeleteJobPrioritizationAsync(request.Id);
            logger.LogInformation("Deleting job prioritization with ID {Id}", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting job prioritization with ID {Id}", request.Id);
            return Result.Error("An error occurred while deleting the job prioritization");
        } 
    }
}