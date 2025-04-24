using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Models.Pagination;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

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

    protected string GetSearchCountCacheKey(string? searchTerm) =>
        string.Format(CacheKeys.SearchCountCacheKey,
            _cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all");

    protected string GetSearchCacheKey(string? searchTerm, Page page) =>
        string.Format(CacheKeys.SearchCacheKey,
            _cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all",
            page.Number,
            page.Size);

    protected (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page)
    {
        var cacheKey = GetSearchCacheKey(searchTerm, page);
        var countCacheKey = GetSearchCountCacheKey(searchTerm);
        return (cacheKey, countCacheKey);
    }

    protected async Task<int?> TryGetCachedSearchCountAsync(string countCacheKey)
    {
        try
        {
            return await _cacheService.GetAsync<int?>(countCacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve cached count for key {CacheKey}", countCacheKey);
            return null;
        }
    }

    protected async Task CacheSearchCountAsync(string countCacheKey, int count)
    {
        try
        {
            await _cacheService.SetAsync(
                key: countCacheKey,
                value: count,
                tags: _listTags,
                expiration: CacheExpirations.MediumTerm);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to cache count for key {CacheKey}", countCacheKey);
        }
    }

    protected async Task<List<TEntity>?> TryGetCachedSearchResults(string cacheKey)
    {
        try
        {
            var cachedResults = await _cacheService.GetAsync<List<TEntity>>(cacheKey);
            if (cachedResults != null)
            {
                _logger.LogDebug("Retrieved {Count} {EntityType} from cache with key {CacheKey}",
                    cachedResults.Count, typeof(TEntity).Name, cacheKey);
            }
            return cachedResults;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve cached search results for key {CacheKey}", cacheKey);
            return null;
        }
    }
}
