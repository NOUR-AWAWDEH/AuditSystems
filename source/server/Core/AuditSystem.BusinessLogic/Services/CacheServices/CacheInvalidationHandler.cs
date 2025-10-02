using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class CacheInvalidationHandler<TId, TEntity, TModel>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags) : EntityCacheHandler<TId, TEntity, TModel>(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags)
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    public async Task InvalidateEntityTagsCacheAsync()
    {
        await cacheService.RemoveCacheByTagAsync(entityTags);
    }

    public async Task ClearUserRelatedCachesAsync(Guid userId, string entityTypeName, Guid? relatedId = null)
    {
        var normalized = entityTypeName.ToLowerInvariant();
        await cacheService.RemoveByPatternAsync($"{normalized}_list_user_{userId}");
        await cacheService.RemoveByPatternAsync($"{normalized}_search_user_{userId}_*");
        await cacheService.RemoveByPatternAsync($"{normalized}_search_count_user_{userId}_*");

        if (relatedId.HasValue)
        {
            await cacheService.RemoveByPatternAsync($"{normalized}_search_user_{userId}_*_{relatedId}_*");
            await cacheService.RemoveByPatternAsync($"{normalized}_search_count_user_{userId}_*_{relatedId}");
        }

        await InvalidateListTagsCacheAsync();
        logger.LogDebug("Cleared caches for {EntityType}, user {UserId}, relatedId {RelatedId}",
            entityTypeName, userId, relatedId);
    }
}