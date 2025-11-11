using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;


namespace AuditSystem.Application.Features.Observation.Get.GetById;

internal sealed class GetObservationByIdQueryHandler(
    IObservationService observationService,
    ILogger<GetObservationByIdQueryHandler> logger) 
    : IRequestHandler<GetObservationByIdQuery, Result<GetObservationByIdQueryResponse>>
{
    public async Task<Result<GetObservationByIdQueryResponse>> Handle(GetObservationByIdQuery request, CancellationToken cancellationToken)
    {
        var observation = await observationService.GetObservationByIdAsync(request.Id, request.UserId);
        
        logger.LogInformation("Observation with id: {ObservationId} has been retrieved successfully by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetObservationByIdQueryResponse>.Success(new GetObservationByIdQueryResponse(observation), "Observation retrieved successfully");
    }
}