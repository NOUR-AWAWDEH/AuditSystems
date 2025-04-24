
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.Cache;

public interface IBaseEntityCacheService<TEntity> where TEntity : class
{
    Task ClearCacheAsync(string cacheKey);
    Task<int?> TryGetCachedSearchCountAsync(string countCacheKey);
    Task CacheSearchCountAsync(string countCacheKey, int count);
    Task<List<TEntity>?> TryGetCachedSearchResults(string cacheKey);
    Task CacheSearchResultsAsync(string cacheKey, IEnumerable<TEntity> entities);

    string GetCacheKey(Guid id, string lang = "en");
    string GetSearchCountCacheKey(string? searchTerm);
    string GetSearchCacheKey(string? searchTerm, Page page);
}
