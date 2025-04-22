using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditUniverseService> logger):
    BaseEntityCacheService<AuditUniverse>(
        cacheService,
        logger,
        CacheKeys.AuditUniverse,
        AuditUniverseTags,
        ListTags
    ), 
    IAuditUniverseService
{
    private static readonly string[] AuditUniverseTags = ["audit-universes", "audit-universe-list"];
    private static readonly string[] ListTags = ["audit-universe-list"];

    public async Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        try
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);

            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditUniverseModel");
            throw new ArgumentException("Mapping failed for AuditUniverseModel.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditUniverse.");
            throw new InvalidCastException("Failed to create AuditUniverse.", ex);
        }
    }
    
    public async Task DeleteAuditUniverseAsync(Guid auditUniverseId)
    {
        var cacheKey = GetCacheKey(auditUniverseId);

        try
        {
            var entity = await repository.GetByIdAsync(auditUniverseId)
                         ?? throw new KeyNotFoundException($"AuditUniverse with ID {auditUniverseId} not found.");

            await repository.DeleteAsync(auditUniverseId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "AuditUniverse with ID {Id} not found", auditUniverseId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditUniverse with ID {Id}", auditUniverseId);
            throw new InvalidOperationException($"Failed to delete AuditUniverse with ID {auditUniverseId}", ex);
        }
    }
    
    public async Task<AuditUniverseModel> GetAuditUniverseByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditUniverse>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditUniverseModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"AuditUniverse with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);

            return mapper.Map<AuditUniverseModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditUniverse with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditUniverse with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        try 
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditUniverse with ID {Id}", auditUniverseModel.Id);
            throw new InvalidOperationException($"Failed to update AuditUniverse with ID '{auditUniverseModel.Id}'", ex);
        }
    }
}
