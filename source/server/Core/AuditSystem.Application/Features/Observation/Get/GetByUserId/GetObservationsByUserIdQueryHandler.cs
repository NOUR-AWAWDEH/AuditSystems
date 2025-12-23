using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Observation.Get.GetByUserId;

internal sealed class GetObservationsByUserIdQueryHandler(
    IObservationService observationService,
    ILogger<GetObservationsByUserIdQueryHandler> logger ) : 
    IRequestHandler<GetObservationsByUserIdQuery, Result<GetObservationsByUserIdQueryResponse>>
{
    public async Task<Result<GetObservationsByUserIdQueryResponse>> Handle(    
        GetObservationsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var observations = await observationService.GetObservationsByUserIdAsync(request.UserId);
        logger.LogDebug("Observations retrieved successfully for user {UserId}", request.UserId);
        return Result<GetObservationsByUserIdQueryResponse>.Success(
            new GetObservationsByUserIdQueryResponse(observations),
            "Observations retrieved successfully");
    }
}
