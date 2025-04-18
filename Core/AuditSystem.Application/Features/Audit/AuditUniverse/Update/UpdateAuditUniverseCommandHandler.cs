using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Update;

internal sealed class UpdateAuditUniverseCommandHandler(
    IAuditUniverseService auditUniverseService,
    IMapper mapper,
    ILogger<UpdateAuditUniverseCommandHandler> logger)
    : IRequestHandler<UpdateAuditUniverseCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditUniverseCommand request, CancellationToken cancellationToken)
    {
        var auditUniverseModel = mapper.Map<AuditUniverseModel>(request);
        await auditUniverseService.UpdateAuditUniverseAsync(auditUniverseModel);

        logger.LogInformation("Audit universe updated successfully");
        return Result.Success();
    }
}