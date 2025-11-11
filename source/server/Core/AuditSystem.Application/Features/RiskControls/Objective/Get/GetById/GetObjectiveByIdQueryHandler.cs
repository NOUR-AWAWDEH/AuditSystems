using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetById;

internal sealed class GetObjectiveByIdQueryHandler(
    IObjectiveService objectiveService,
    ILogger<GetObjectiveByIdQueryHandler> logger) : 
    IRequestHandler<GetObjectiveByIdQuery, Result<GetObjectiveByIdQueryResponse>>
{
    public async Task<Result<GetObjectiveByIdQueryResponse>> Handle(GetObjectiveByIdQuery request, CancellationToken cancellationToken)
    {
        var objective = await objectiveService.GetObjectiveByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Objective with id: {Id} was retrieved by UserId: {UserId}", request.Id, request.UserId);
        return Result<GetObjectiveByIdQueryResponse>.Success(new GetObjectiveByIdQueryResponse(objective));
    }
}
