using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditDomainService(
    IRepository<Guid, AuditDomain> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditDomainService
{
    private static readonly string[] AuditDomainTags = ["audit-domains", "audit-domain-list"];
    private static readonly string[] ListTags = ["audit-domain-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditDomainAsync(AuditDomainModel auditDomianModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomianModel, nameof(auditDomianModel));

        try
        {
            var entity = mapper.Map<AuditDomain>(auditDomianModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditDomain, createdEntity.Id);

            await cacheService.SetAsync(
               key: cacheKey,
               value: createdEntity,
               tags: AuditDomainTags,
               expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditDomain.", ex);
        }
    }
}
