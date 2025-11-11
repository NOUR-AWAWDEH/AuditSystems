using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

namespace AuditSystem.Application.Features.Observation.Create;

internal sealed class CreateObservationCommandHandler(
    IObservationService observationService,
    IMapper mapper,
    ILogger<CreateObservationCommandHandler> logger)
    : IRequestHandler<CreateObservationCommand, Result<CreateObservationCommandResponse>>
{
    public async Task<Result<CreateObservationCommandResponse>> Handle(CreateObservationCommand request, CancellationToken cancellationToken)
    {
        var observationModel = mapper.Map<ObservationModel>(request);
        var observationId = await observationService.CreateObservationAsync(observationModel, request.UserId);
        
        logger.LogInformation("Observation with Description {ObservationDescription} has been created by user with id: {UserId}", request.Description, request.UserId);
        return Result<CreateObservationCommandResponse>.Created(new(observationId), "Observation has been created.");
    }
}