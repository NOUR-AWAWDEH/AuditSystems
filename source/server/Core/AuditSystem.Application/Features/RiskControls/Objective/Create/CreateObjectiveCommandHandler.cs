using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.Objective.Create;

internal sealed class CreateObjectiveCommandHandler(
    IObjectiveService objectiveService,
    IMapper mapper,
    ILogger<CreateObjectiveCommandHandler> logger)
    : IRequestHandler<CreateObjectiveCommand, Result<CreateObjectiveCommandResponse>>
{
    public async Task<Result<CreateObjectiveCommandResponse>> Handle(CreateObjectiveCommand request, CancellationToken cancellationToken)
    {
        var objectiveModel = mapper.Map<ObjectiveModel>(request);
        var objectiveId = await objectiveService.CreateObjectiveAsync(objectiveModel, request.UserId);

        logger.LogInformation("Objective with ID {ObjectiveId} has been created by userID {UserID}.", objectiveId, request.UserId);
        return Result<CreateObjectiveCommandResponse>.Created(new CreateObjectiveCommandResponse(objectiveId), "Objective created.");
    }
}
