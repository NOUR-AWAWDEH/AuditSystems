using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Models.Processes;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.SubProcess.Update;

internal sealed class UpdateSubProcessCommandHandler(
    ISubProcessService subProcessService,
    IMapper mapper,
    ILogger<UpdateSubProcessCommandHandler> logger)
    : IRequestHandler<UpdateSubProcessCommand, Result>
{
    public async Task<Result> Handle(UpdateSubProcessCommand request, CancellationToken cancellationToken)
    {
        var subProcessModel = mapper.Map<SubProcessModel>(request);
        await subProcessService.UpdateSubProcessAsync(subProcessModel);

        logger.LogInformation("SubProcess updated successfully");
        return Result.Success();
    }
}