using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Models.Observation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Observation.Update;

public sealed class UpdateObservationCommandHandler(
    IObservationService observationService,
    IMapper mapper,
    ILogger<UpdateObservationCommandHandler> logger)
    : IRequestHandler<UpdateObservationCommand, Result>
{
    public async Task<Result> Handle(UpdateObservationCommand request, CancellationToken cancellationToken)
    {
        var observationModel = mapper.Map<ObservationModel>(request);
        await observationService.UpdateObservationAsync(observationModel, request.UserId);

        logger.LogInformation("Observation with ID: {ObservationId} updated by User Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Observation updated successfully");
    }
}
