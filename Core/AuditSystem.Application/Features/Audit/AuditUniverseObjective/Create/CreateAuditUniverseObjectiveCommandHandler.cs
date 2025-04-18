using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;

internal sealed class CreateAuditUniverseObjectiveCommandHandler(
    IAuditUniverseObjectiveService auditUniverseObjectiveService,
    IMapper mapper,
    ILogger<CreateAuditUniverseObjectiveCommandHandler> logger)
    : IRequestHandler<CreateAuditUniverseObjectiveCommand, Result<CreateAuditUniverseObjectiveCommandResponse>>
{
    public async Task<Result<CreateAuditUniverseObjectiveCommandResponse>> Handle(CreateAuditUniverseObjectiveCommand request, CancellationToken cancellationToken)
    {
        var auditUniverseObjectiveModel = mapper.Map<AuditUniverseObjectiveModel>(request);
        var auditUniverseObjectiveId = await auditUniverseObjectiveService.CreateAuditUniverseObjectiveAsync(auditUniverseObjectiveModel);
        logger.LogInformation("Audit Universe Objective with Impact {AuditUniverseObjectiveImpact} has been created.", request.Impact);

        return Result<CreateAuditUniverseObjectiveCommandResponse>.Created(new CreateAuditUniverseObjectiveCommandResponse(auditUniverseObjectiveId));
    }
}
