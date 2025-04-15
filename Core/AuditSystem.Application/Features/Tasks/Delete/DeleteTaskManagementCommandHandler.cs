using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Tasks.Delete;

internal sealed class DeleteTaskManagementCommandHandler( 
    ITaskManagementService taskManagementService,
    ILogger<DeleteTaskManagementCommandHandler> logger) :
    IRequestHandler<DeleteTaskManagementCommand, Result>
{
    public async Task<Result> Handle(DeleteTaskManagementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await taskManagementService.DeleteTaskAsync(request.Id);
            logger.LogInformation("Task management with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting task management with id: {Id}", request.Id);
            return Result.Error("Error deleting task management");
        }      
    }
}

