using AuditSystem.Application.Constants;
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
    private static readonly string[] ComplianceAuditLinkTags = ["compliance-audit-links", "compliance-audit-link-list"];
    private static readonly string[] ListTags = ["compliance-audit-link-list"]; // Tags for collections only

    public async Task<Guid> CreateComplianceAuditLinkAsync(ComplianceAuditLinkModel complianceAuditLinkModel)
    {
        ArgumentNullException.ThrowIfNull(complianceAuditLinkModel, nameof(complianceAuditLinkModel));
        
        try
        {
            var entity = mapper.Map<ComplianceAuditLink>(complianceAuditLinkModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.ComplianceAuditLink, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ComplianceAuditLinkTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ComplianceAuditLink.", ex);
        }
    }
}
