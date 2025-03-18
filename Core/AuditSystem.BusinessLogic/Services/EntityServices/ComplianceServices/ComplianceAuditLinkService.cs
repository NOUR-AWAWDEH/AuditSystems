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
    public async Task<Guid> CreateComplianceAuditLinkAsync(ComplianceAuditLinkModel complianceAuditLinkModel)
    {
        ArgumentNullException.ThrowIfNull(complianceAuditLinkModel, nameof(complianceAuditLinkModel));
        
        try
        {
            var entity = mapper.Map<ComplianceAuditLink>(complianceAuditLinkModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
