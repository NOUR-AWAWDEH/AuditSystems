using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using Microsoft.Extensions.Logging;
using MediatR;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Delete;

internal sealed class DeleteJobSchedulingCommandHandler(
    IJobSchedulingService jobSchedulingService,
    ILogger<DeleteJobSchedulingCommandHandler> logger) :
    IRequestHandler<DeleteJobSchedulingCommand, Result>
{
    public async Task<Result> Handle(DeleteJobSchedulingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await jobSchedulingService.DeleteJobSchedulingAsync(request.Id);
            logger.LogInformation("Deleting job scheduling with ID {Id}", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting job scheduling with ID {Id}", request.Id);
            return Result.Error(ex.Message);  
        } 
    }
}