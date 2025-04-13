using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.Objective.Update;

internal sealed class UpdateObjectiveCommandHandler(
    IObjectiveService objectiveService,
    IMapper mapper,
    ILogger<UpdateObjectiveCommandHandler> logger)
    : IRequestHandler<UpdateObjectiveCommand, Result>
{
    public async Task<Result> Handle(UpdateObjectiveCommand request, CancellationToken cancellationToken)
    {
        var objectiveModel = mapper.Map<ObjectiveModel>(request);
        await objectiveService.UpdateObjectiveAsync(objectiveModel);

        logger.LogInformation("Objective updated successfully");
        return Result.Success();
    }
}