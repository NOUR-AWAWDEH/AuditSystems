using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Models.Processes;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Update;

internal sealed class UpdateAuditProcessCommandHandler(
    IAuditProcessService auditProcessService,
    IMapper mapper,
    ILogger<UpdateAuditProcessCommandHandler> logger)
    : IRequestHandler<UpdateAuditProcessCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditProcessCommand request, CancellationToken cancellationToken)
    {
        var auditProcessModel = mapper.Map<AuditProcessModel>(request);
        await auditProcessService.UpdateAuditProcessAsync(auditProcessModel);
        
        logger.LogInformation("Audit process updated successfully");
        return Result.Success();
    }
}