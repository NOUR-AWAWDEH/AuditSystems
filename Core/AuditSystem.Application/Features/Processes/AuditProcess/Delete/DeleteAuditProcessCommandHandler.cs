using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Delete;

internal sealed class DeleteAuditProcessCommandHandler( 
    IAuditProcessService auditProcessService,
    ILogger<DeleteAuditProcessCommandHandler> logger) :
    IRequestHandler<DeleteAuditProcessCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditProcessCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditProcessService.DeleteAuditProcessAsync(request.Id);
            logger.LogInformation("Audit process with id: {Id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting audit process with id: {Id}", request.Id);
            return Result.Error(ex.Message);
        }  
    }
}

