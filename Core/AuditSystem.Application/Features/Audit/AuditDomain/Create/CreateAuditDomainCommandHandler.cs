using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Create;

internal sealed class CreateAuditDomainCommandHandler(
    IAuditDomainService auditDomainService,
    IMapper mapper,
    ILogger<CreateAuditDomainCommandHandler> logger)
    : IRequestHandler<CreateAuditDomainCommand, Result<CreateAuditDomainCommandResponse>>

{

    public async Task<Result<CreateAuditDomainCommandResponse>> Handle(CreateAuditDomainCommand request, CancellationToken cancellationToken)
    {
        var DomainModel = mapper.Map<AuditDomainModel>(request);
        var DomainId = await auditDomainService.CreateAuditDomainAsync(DomainModel);
        logger.LogInformation("Audit Domain with name {Name} has been created.", request.Name);

        return Result<CreateAuditDomainCommandResponse>.Created(new(DomainId), "Audit Domain created.");
    }
}
