using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceAuditLinkService(
    IRepository<Guid, ComplianceAuditLink> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IComplianceAuditLinkService
{
    public Task<Guid> CreateComplianceAuditLinkAsync(ComplianceAuditLinkModel complianceAuditLinkModel)
    {
        throw new NotImplementedException();
    }
}
