using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Delete;

internal sealed class DeleteAuditProcessCommandHandler( 
    IAuditProcessService auditProcessService,
    ILogger<DeleteAuditProcessCommandHandler> logger) :
    IRequestHandler<DeleteAuditProcessCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditProcessCommand request, CancellationToken cancellationToken)
    {
        await auditProcessService.DeleteAuditProcessAsync(request.Id, request.UserId);
        
        logger.LogInformation("Audit process with id: {Id} was deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Audit process deleted");  
    }
}