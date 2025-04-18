using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Delete;

internal sealed class DeleteAuditUniverseCommandHandler(
    IAuditUniverseService auditUniverseService,
    ILogger<DeleteAuditUniverseCommandHandler> logger) : 
    IRequestHandler<DeleteAuditUniverseCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditUniverseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditUniverseService.DeleteAuditUniverseAsync(request.Id);
            logger.LogInformation("Deleting audit universe with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting audit universe with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}
