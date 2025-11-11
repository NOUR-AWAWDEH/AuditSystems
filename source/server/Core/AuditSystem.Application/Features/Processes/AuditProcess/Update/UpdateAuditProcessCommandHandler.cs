using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
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
        
        await auditProcessService.UpdateAuditProcessAsync(auditProcessModel, request.UserId);
        
        logger.LogInformation("Audit process updated successfully with ID: {AuditProcessId} by user Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Audit process updated successfully");
    }
}