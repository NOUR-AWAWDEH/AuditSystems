using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Create;

internal sealed class CreateBusinessObjectiveCommandHandler(
    IBusinessObjectiveService businessObjectiveService,
    IMapper mapper,
    ILogger<CreateBusinessObjectiveCommandHandler> logger)
    : IRequestHandler<CreateBusinessObjectiveCommand, Result<CreateBusinessObjectiveCommandResponse>>
{
    public async Task<Result<CreateBusinessObjectiveCommandResponse>> Handle(CreateBusinessObjectiveCommand request, CancellationToken cancellationToken)
    {
        var businessObjectiveModel = mapper.Map<BusinessObjectiveModel>(request);
        var businessObjectiveId = await businessObjectiveService.CreateBusinessObjectiveAsync(businessObjectiveModel);
        logger.LogInformation("Business Objective with Impact {BusinessObjectiveImpact} has been created.", request.Impact);

        return Result<CreateBusinessObjectiveCommandResponse>.Created(new CreateBusinessObjectiveCommandResponse(businessObjectiveId));
    }
}
