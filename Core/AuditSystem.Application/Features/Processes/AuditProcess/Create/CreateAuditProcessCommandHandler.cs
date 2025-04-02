using Ardalis.Result;
using AuditSystem.Application.Features.Processes.AuditProcess.Creat;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Models.Processes;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

internal sealed class CreateAuditProcessCommandHandler(
    IAuditProcessService auditProcessService,
    IMapper mapper,
    ILogger<CreateAuditProcessCommandHandler> logger)
    : IRequestHandler<CreateAuditProcessCommand, Result<CreateAuditProcessCommandResponse>>
{
    public async Task<Result<CreateAuditProcessCommandResponse>> Handle(CreateAuditProcessCommand request, CancellationToken cancellationToken)
    {
        var auditProcessModel = mapper.Map<AuditProcessModel>(request);
        var auditProcessId = await auditProcessService.CreateAuditProcessAsync(auditProcessModel);
        logger.LogInformation("Audit Process with Process Name {AuditProcessName} has been created.", request.ProcessName);

        return Result<CreateAuditProcessCommandResponse>.Created(new CreateAuditProcessCommandResponse(auditProcessId));
    }
}
