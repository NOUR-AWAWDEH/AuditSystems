using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class EntityCacheHandler<TId, TEntity, TModel>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags)
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    protected readonly IRepository<TId, TEntity> repository = repository;
    protected readonly IMapper mapper = mapper;
    protected readonly ICacheService cacheService = cacheService;
    protected readonly ILogger logger = logger;
    protected readonly string cacheKeyPrefix = cacheKeyPrefix;
    protected readonly string[] entityTags = entityTags;
    protected readonly string[] listTags = listTags;

    public string GetCacheKey(TId id, string lang = "en") =>
        string.Format(CacheKeys.CacheKey, cacheKeyPrefix, id, lang);

    public async Task CacheEntityAsync(TEntity entity)
    {
        var key = GetCacheKey(entity.Id);
        await cacheService.SetAsync(key, entity, tags: entityTags, expiration: CacheExpirations.MediumTerm);
        await InvalidateListTagsCacheAsync();
    }

    // Generic version (returns TTargetModel)

    public async Task<TTargetModel?> TryGetFromCacheWithValidationAsync<TTargetModel>(
    TId id,
    Guid userId,
    Expression<Func<TEntity, Guid>> userIdSelector,
    string unauthorizedMessage)
    where TTargetModel : class
    {
        try
        {
            var cacheKey = GetCacheKey(id);
            var cached = await cacheService.GetAsync<TEntity>(cacheKey);
            if (cached == null)
            {
                logger.LogDebug("Cache miss for entity {EntityId}", id);
                return default;
            }

            // Safe execution of the selector
            Guid cachedUserId;
            try
            {
                var compiledSelector = userIdSelector.Compile();
                cachedUserId = compiledSelector(cached);
            }
            catch (NullReferenceException ex)
            {
                logger.LogWarning(ex, "Null reference in cached entity {EntityId} - relationships not properly cached", id);
                return default;
            }

            if (cachedUserId == Guid.Empty || cachedUserId != userId)
            {
                logger.LogWarning("User {UserId} not authorized to access cached entity {EntityId} owned by {CachedUserId}",
                    userId, id, cachedUserId);
                throw new UnauthorizedAccessException(string.Format(unauthorizedMessage, userId, id));
            }

            logger.LogDebug("Cache hit for entity {EntityId}", id);
            return mapper.Map<TTargetModel>(cached);
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException)
        {
            logger.LogWarning(ex, "Error during cache validation for entity {EntityId}", id);
            return default;
        }
    }

    public async Task ClearEntityCacheAsync(TId id)
    {
        var cacheKey = GetCacheKey(id);
        await cacheService.RemoveByPatternAsync(cacheKey);
        await InvalidateListTagsCacheAsync();
    }

    protected async Task InvalidateListTagsCacheAsync()
    {
        await cacheService.RemoveCacheByTagAsync(listTags);
    }
}