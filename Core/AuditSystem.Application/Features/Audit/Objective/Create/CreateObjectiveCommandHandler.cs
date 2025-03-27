using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.Objective.Create;

internal sealed class CreateObjectiveCommandHandler(
    IObjectiveService objectiveService,
    IMapper mapper,
    ILogger<CreateObjectiveCommandHandler> logger)
    : IRequestHandler<CreateObjectiveCommand, Result<CreateObjectiveCommandResponse>>
{
    public async Task<Result<CreateObjectiveCommandResponse>> Handle(CreateObjectiveCommand request, CancellationToken cancellationToken)
    {
        var objectiveModel = mapper.Map<ObjectiveModel>(request);
        var objectiveId = await objectiveService.CreateObjectiveAsync(objectiveModel);
        logger.LogInformation("Objective with RatingID {ObjectiveRatingId} has been created.", request.RatingId);

        return Result<CreateObjectiveCommandResponse>.Created(new CreateObjectiveCommandResponse(objectiveId));
    }
}
