using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Models.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Tasks.Update;

internal sealed class UpdateTaskManagementCommandHandler(
    ITaskManagementService taskManagementService,
    IMapper mapper,
    ILogger<UpdateTaskManagementCommandHandler> logger)
    : IRequestHandler<UpdateTaskManagementCommand, Result>
{
    public async Task<Result> Handle(UpdateTaskManagementCommand request, CancellationToken cancellationToken)
    {
        var taskManagementModel = mapper.Map<TaskManagementModel>(request);
        await taskManagementService.UpdateTaskAsync(taskManagementModel);
        
        logger.LogInformation("Task management with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}
