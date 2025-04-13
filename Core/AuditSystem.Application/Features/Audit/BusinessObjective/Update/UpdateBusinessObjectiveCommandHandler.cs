using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Update;

internal sealed class UpdateBusinessObjectiveCommandHandler(
    IBusinessObjectiveService businessObjectiveService,
    IMapper mapper,
    ILogger<UpdateBusinessObjectiveCommandHandler> logger)
    : IRequestHandler<UpdateBusinessObjectiveCommand, Result>
{
    public async Task<Result> Handle(UpdateBusinessObjectiveCommand request, CancellationToken cancellationToken)
    {
        var businessObjectiveModel = mapper.Map<BusinessObjectiveModel>(request);
        await businessObjectiveService.UpdateBusinessObjectiveAsync(businessObjectiveModel);

        logger.LogInformation("Business objective updated successfully");
        return Result.Success();
    }
}

