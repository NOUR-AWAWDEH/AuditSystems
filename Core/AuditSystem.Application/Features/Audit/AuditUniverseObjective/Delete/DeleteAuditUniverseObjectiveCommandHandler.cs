using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Delete;

internal sealed class DeleteAuditUniverseObjectiveCommandHandler(
    IAuditUniverseObjectiveService auditUniverseObjectiveService,
    ILogger<DeleteAuditUniverseObjectiveCommandHandler> logger) :
    IRequestHandler<DeleteAuditUniverseObjectiveCommand, Result>
{
    public async Task<Result> Handle(DeleteAuditUniverseObjectiveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await auditUniverseObjectiveService.DeleteAuditUniverseObjectiveAsync(request.Id);
            logger.LogInformation("Deleting audit universe objective with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting audit universe objective with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}
