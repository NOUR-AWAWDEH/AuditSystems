using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Models.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Tasks.Create;

internal sealed class CreateTaskManagementCommandHandler(
    ITaskManagementService taskManagementService,
    IMapper mapper,
    ILogger<CreateTaskManagementCommandHandler> logger)
    : IRequestHandler<CreateTaskManagementCommand, Result<CreateTaskManagementCommandResponse>>
{
    public async Task<Result<CreateTaskManagementCommandResponse>> Handle(CreateTaskManagementCommand request, CancellationToken cancellationToken)
    {
        var taskManagementModel = mapper.Map<TaskManagementModel>(request);
        var taskManagementId = await taskManagementService.CreateTaskAsync(taskManagementModel);
        logger.LogInformation("Task Management with Job Name {TaskManagementJobName} has been created.", request.JobName);

        return Result<CreateTaskManagementCommandResponse>.Created(new CreateTaskManagementCommandResponse(taskManagementId));
    }
}