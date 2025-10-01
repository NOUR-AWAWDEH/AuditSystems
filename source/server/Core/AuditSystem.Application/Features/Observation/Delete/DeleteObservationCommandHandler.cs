using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

namespace AuditSystem.Application.Features.Observation.Delete;

internal sealed class DeleteObservationCommandHandler(
    IObservationService observationService,
    ILogger<DeleteObservationCommandHandler> logger) :
    IRequestHandler<DeleteObservationCommand, Result>
{
    public async Task<Result> Handle(DeleteObservationCommand request, CancellationToken cancellationToken)
    {
        await observationService.DeleteObservationAsync(request.Id, request.UserId);
        logger.LogInformation("Observation with ID {Id} has been deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Observation deleted successfully");
    }
}