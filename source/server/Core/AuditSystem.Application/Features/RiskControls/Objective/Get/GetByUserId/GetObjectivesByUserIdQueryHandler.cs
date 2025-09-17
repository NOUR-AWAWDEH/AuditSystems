using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetByUserId;

internal sealed class GetObjectivesByUserIdQueryHandler(
    IObjectiveService objectiveService,
    ILogger<GetObjectivesByUserIdQueryHandler> logger) 
    : IRequestHandler<GetObjectivesByUserIdQuery, Result<GetObjectivesByUserIdQueryResponse>>
{
    public async Task<Result<GetObjectivesByUserIdQueryResponse>> Handle(
        GetObjectivesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var objectives = await objectiveService.GetObjectivesByUserIdAsync(request.UserId);

        logger.LogInformation("Objectives with user id: {UserId} was retrieved", request.UserId);
        return Result<GetObjectivesByUserIdQueryResponse>.Success(new GetObjectivesByUserIdQueryResponse(objectives.ToList()), "Objectives retrieved successfully");
    }
}