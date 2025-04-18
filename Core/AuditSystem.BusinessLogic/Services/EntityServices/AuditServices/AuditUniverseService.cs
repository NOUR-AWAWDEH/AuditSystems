using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseService
{
    private static readonly string[] AuditUniverseTags = ["audit-universes", "audit-universe-list"];
    private static readonly string[] ListTags = ["audit-universe-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        try
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditUniverse, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditUniverseTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditUniverse.", ex);
        }
    }
    public async Task DeleteAuditUniverseAsync(Guid auditUniverseId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(auditUniverseId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditUniverse with ID {auditUniverseId} not found.");
            }

            await repository.DeleteAsync(auditUniverseId);

            var cacheKey = string.Format(CacheKeys.AuditUniverse, auditUniverseId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
           throw new Exception("Failed to delete AuditUniverse.", ex);
        }
    }
    public async Task<AuditUniverseModel> GetAuditUniverseByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditUniverse, id);
            
            var cachedAuditUniverseModel = await cacheService.GetAsync<AuditUniverse>(cacheKey);
            if (cachedAuditUniverseModel != null)
            { 
                return mapper.Map<AuditUniverseModel>(cachedAuditUniverseModel);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null) 
            {
                throw new KeyNotFoundException($"AuditUniverse with ID {id} not found.");    
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditUniverseTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<AuditUniverseModel>(entity);  
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditUniverse by ID {id}.", ex);
        }
    }
    public async Task UpdateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        try 
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditUniverse, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditUniverseTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditUniverse.", ex);
        }
    }
}
