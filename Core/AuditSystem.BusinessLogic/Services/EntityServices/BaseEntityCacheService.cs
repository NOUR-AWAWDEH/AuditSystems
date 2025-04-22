using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices;

public abstract class BaseEntityCacheService<TEntity>(
    ICacheService cacheService,
    ILogger<BaseEntityCacheService<TEntity>> logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags)
    where TEntity : class
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<BaseEntityCacheService<TEntity>> _logger = logger;
    private readonly string _cacheKeyPrefix = cacheKeyPrefix;
    private readonly string[] _entityTags = entityTags;
    private readonly string[] _listTags = listTags;

    protected string GetCacheKey(Guid id, string lang = "en") =>
        string.Format(CacheKeys.CacheKey, _cacheKeyPrefix, id, lang);

    protected async Task SetCacheAsync(string cacheKey, TEntity entity)
    {
        try
        {
            await _cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: _entityTags,
                expiration: CacheExpirations.MediumTerm);

            await _cacheService.RemoveCacheByTagAsync(_listTags);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to update cache for {EntityType} with ID {Id}",
                typeof(TEntity).Name, GetEntityId(entity));
        }
    }

    protected async Task ClearCacheAsync(string cacheKey)
    {
        try
        {
            await _cacheService.RemoveAsync(cacheKey);
            await _cacheService.RemoveCacheByTagAsync(_listTags);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clear cache for key {CacheKey}", cacheKey);
        }
    }

    private Guid GetEntityId(TEntity entity)
    {
        var prop = typeof(TEntity).GetProperty("Id");
        return prop != null ? (Guid)prop.GetValue(entity)! : Guid.Empty;
    }
}