using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;

internal sealed class UpdateAuditUniverseObjectiveCommandHandler(
    IAuditUniverseObjectiveService auditUniverseObjectiveService,
    IMapper mapper,
    ILogger<UpdateAuditUniverseObjectiveCommandHandler> logger)
    : IRequestHandler<UpdateAuditUniverseObjectiveCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditUniverseObjectiveCommand request, CancellationToken cancellationToken)
    {
        var auditUniverseObjectiveModel = mapper.Map<AuditUniverseObjectiveModel>(request);
        await auditUniverseObjectiveService.UpdateAuditUniverseObjectiveAsync(auditUniverseObjectiveModel);

        logger.LogInformation("Audit universe objective updated successfully");
        return Result.Success();
    }
}
