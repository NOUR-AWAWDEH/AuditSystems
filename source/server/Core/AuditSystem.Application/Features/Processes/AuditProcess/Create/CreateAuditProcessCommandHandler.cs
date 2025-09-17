using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
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
        var auditProcessId = await auditProcessService.CreateAuditProcessAsync(auditProcessModel, request.UserId);
        
        logger.LogInformation("Audit Process with Process Name {AuditProcessName} has been created by user with id: {UserId}", request.ProcessName, request.UserId);
        return Result<CreateAuditProcessCommandResponse>.Created(new CreateAuditProcessCommandResponse(auditProcessId), "Audit Process created.");
    }
}