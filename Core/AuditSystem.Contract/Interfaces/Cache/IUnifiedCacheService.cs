using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Interfaces.Cache;

public interface IUnifiedCacheService<TId, TEntity>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    Task<PaginationResult<TModel>?> GetFromCacheAsync<TModel>(string cacheKey, string countCacheKey, Page page);
    Task<PaginationResult<TModel>> GetFromRepositoryAsync<TModel>(string cacheKey, string countCacheKey, Page page, string? name);
    Task ClearCacheAsync(string cacheKey);
    Task<int?> TryGetCachedSearchCountAsync(string countCacheKey);
    Task CacheSearchCountAsync(string countCacheKey, int count);
    Task<List<TEntity>?> TryGetCachedSearchResults(string cacheKey);
    Task CacheSearchResultsAsync(string cacheKey, IEnumerable<TEntity> entities);
    string GetCacheKey(TId id, string lang = "en");
    string GetSearchCountCacheKey(string? searchTerm);
    string GetSearchCacheKey(string? searchTerm, Page page);
    Task SetCacheAsync(string cacheKey, TEntity entity);
    (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page);
}