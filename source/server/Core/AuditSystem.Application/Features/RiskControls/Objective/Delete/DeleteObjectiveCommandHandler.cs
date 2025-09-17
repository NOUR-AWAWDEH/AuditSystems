using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;

namespace AuditSystem.Application.Features.RiskControls.Objective.Delete;

internal class DeleteObjectiveCommandHandler(
    IObjectiveService objectiveService,
    ILogger<DeleteObjectiveCommandHandler> logger
) : IRequestHandler<DeleteObjectiveCommand, Result>
{
    public async Task<Result> Handle(DeleteObjectiveCommand request, CancellationToken cancellationToken)
    {
        await objectiveService.DeleteObjectiveAsync(request.Id, request.UserId);
        logger.LogInformation("Deleting objective with ID {Id} was deleted by UserId: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Objective deleted successfully.");

    }
}