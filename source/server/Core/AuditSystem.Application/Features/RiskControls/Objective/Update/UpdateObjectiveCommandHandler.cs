using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Models.RiskControls;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.Objective.Update;

internal sealed class UpdateObjectiveCommandHandler(
    IObjectiveService objectiveService,
    IMapper mapper,
    ILogger<UpdateObjectiveCommandHandler> logger)
    : IRequestHandler<UpdateObjectiveCommand, Result>
{
    public async Task<Result> Handle(
        UpdateObjectiveCommand request, 
        CancellationToken cancellationToken)
    {
        var objectiveModel = mapper.Map<ObjectiveModel>(request);
        await objectiveService.UpdateObjectiveAsync(objectiveModel, request.UserId);

        logger.LogInformation("Objective updated successfully with id: {Id} by user id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Objective updated successfully");
    }
}