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

    public async Task<Guid> CreateAuditDomainAsync(AuditDomainModel auditDomainModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomainModel, nameof(auditDomainModel));

        try
        {
            var entity = mapper.Map<AuditDomain>(auditDomainModel);
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
    
    public async Task DeleteAuditDomainAsync(Guid auditDomainId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(auditDomainId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditEngagement with ID {auditDomainId} not found.");
            }

            await repository.DeleteAsync(auditDomainId);
            
            var cacheKey = string.Format(CacheKeys.AuditDomain, auditDomainId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditDomain.", ex);
        }
    }
    
    public async Task<AuditDomainModel> GetAuditDomainByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditDomain, id);
            
            // Try to get from cache first
            var cachedEntity = await cacheService.GetAsync<AuditDomain>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditDomainModel>(cachedEntity);
            }

            // Cache miss - get from repository
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditDomain with ID {id} not found.");
            }

            // Store entity in cache
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditDomainTags,
                expiration: CacheExpirations.MediumTerm
            );

            // Map to model and return
            return mapper.Map<AuditDomainModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditDomain with ID {id}.", ex);
        }
    }
    public async Task UpdateAuditDomainAsync(AuditDomainModel auditDomianModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomianModel, nameof(auditDomianModel));
        
        try 
        {
            var entity = mapper.Map<AuditDomain>(auditDomianModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditDomain, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditDomainTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditDomain.", ex);
        }
    }
}
