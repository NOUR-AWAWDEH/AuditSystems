using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Models.Processes;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.SubProcess.Create;

internal sealed class CreateSubProcessCommandHandler(
    ISubProcessService subProcessService,
    IMapper mapper,
    ILogger<CreateSubProcessCommandHandler> logger)
    : IRequestHandler<CreateSubProcessCommand, Result<CreateSubProcessCommandResponse>>
{
    public async Task<Result<CreateSubProcessCommandResponse>> Handle(CreateSubProcessCommand request, CancellationToken cancellationToken)
    {
        var subProcessModel = mapper.Map<SubProcessModel>(request);
        var subProcessId = await subProcessService.CreateSubProcessAsync(subProcessModel);
        logger.LogInformation("SubProcess with  Particular {SubProcessParticular} has been created.", request.Particular);

        return Result<CreateSubProcessCommandResponse>.Created(new CreateSubProcessCommandResponse(subProcessId));
    }
}
