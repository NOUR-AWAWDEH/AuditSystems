using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseObjectiveService(
    IRepository<Guid, AuditUniverseObjective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditUniverseObjectiveService> logger) :
    BaseEntityCacheService<AuditUniverseObjective>
    (
        cacheService,
        logger,
        CacheKeys.AuditUniverseObjective,
        AuditUniverseObjectiveTags,
        ListTags
    ),
    IAuditUniverseObjectiveService

{
    private static readonly string[] AuditUniverseObjectiveTags = ["audit-universe-objectives", "audit-universe-objective-list"];
    private static readonly string[] ListTags = ["audit-universe-objective-list"];

    public async Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));

        try
        {
            var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditUniverseObjectiveModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditUniverseObjective");
            throw new InvalidCastException("Failed to create AuditUniverseObjective.", ex);
        }
    }

    public async Task DeleteAuditUniverseObjectiveAsync(Guid auditUniverseObjectiveId)
    {
        var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.AuditUniverseObjective, auditUniverseObjectiveId, "en");
        
        try
        {
            var entity = await repository.GetByIdAsync(auditUniverseObjectiveId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditUniverseObjective with ID {auditUniverseObjectiveId} not found.");
            }

            await repository.DeleteAsync(auditUniverseObjectiveId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditUniverseObjective.", ex);
        }
    }

    public async Task<AuditUniverseObjectiveModel> GetAuditUniverseObjectiveByIdAsync(Guid id)
    {           
        var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.AuditUniverseObjective, id, "en");

        try
        {            
            var cachedModel = await cacheService.GetAsync<AuditUniverseObjective>(cacheKey);
            if (cachedModel != null)   
            {
                return mapper.Map<AuditUniverseObjectiveModel>(cachedModel);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Audit Universe Objective with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditUniverseObjectiveTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<AuditUniverseObjectiveModel>(entity);  
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get Audit Universe Objective ID {id}. Error: {ex.Message}", ex);
        }
    }

    public async Task UpdateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));
        
        try
        {
            var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditUniverseObjective, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditUniverseObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditUniverseObjective.", ex);
        }
    }
}
