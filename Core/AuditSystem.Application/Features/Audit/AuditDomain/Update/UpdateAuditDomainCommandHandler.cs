using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Update;

internal sealed class UpdateAuditDomainCommandHandler(
    IAuditDomainService auditDomainService,
    IMapper mapper,
    ILogger<UpdateAuditDomainCommandHandler> logger)
    : IRequestHandler<UpdateAuditDomainCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditDomainCommand request, CancellationToken cancellationToken)
    {
        var auditDomainModel = mapper.Map<AuditDomainModel>(request);
        await auditDomainService.UpdateAuditDomainAsync(auditDomainModel);

        logger.LogInformation("Audit domain updated successfully");
        return Result.Success();
    }
}