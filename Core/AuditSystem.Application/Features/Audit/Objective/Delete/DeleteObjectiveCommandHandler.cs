using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Audit.Objective.Delete;

internal class DeleteObjectiveCommandHandler(
    IObjectiveService objectiveService,
    ILogger<DeleteObjectiveCommandHandler> logger
) : IRequestHandler<DeleteObjectiveCommand, Result>
{
    public async Task<Result> Handle(DeleteObjectiveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await objectiveService.DeleteObjectiveAsync(request.Id);
            logger.LogInformation("Deleting objective with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting objective with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        }
    }
}