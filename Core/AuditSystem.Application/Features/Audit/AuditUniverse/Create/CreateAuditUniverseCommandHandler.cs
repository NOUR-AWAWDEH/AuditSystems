using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Create;

internal sealed class CreateAuditUniverseCommandHandler(
    IAuditUniverseService auditUniverseService,
    IMapper mapper,
    ILogger<CreateAuditUniverseCommandHandler> logger) :
    IRequestHandler<CreateAuditUniverseCommand, Result<CreateAuditUniverseCommandResponse>>
{
    public async Task<Result<CreateAuditUniverseCommandResponse>> Handle(CreateAuditUniverseCommand command, CancellationToken cancellationToken)
    {
        var auditUniverseModel = mapper.Map<AuditUniverseModel>(command);
        var auditUniverseId = await auditUniverseService.CreateAuditUniverseAsync(auditUniverseModel);
        logger.LogInformation("AuditUniverse with Business Objective {BusinessObjective} has been created.", command.BusinessObjective);

        return Result<CreateAuditUniverseCommandResponse>.Created(new CreateAuditUniverseCommandResponse(auditUniverseId));
    }
}