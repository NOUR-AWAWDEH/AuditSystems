using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using System.Linq.Expressions;

namespace AuditSystem.Contract.Interfaces.Cache;

public interface IUnifiedCacheService<TId, TEntity, TModel>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    Task<PaginationResult<TTargetModel>?> GetFromCacheAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page);
    Task<PaginationResult<TTargetModel>> GetFromRepositoryAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page, string? name);
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
    Task<TTargetModel?> TryGetFromCacheWithValidationAsync<TTargetModel>(TId id, Guid userId, Expression<Func<TEntity, Guid>> userIdSelector, string unauthorizedMessage);
    Task CacheEntityAsync(TEntity entity);
    Task ClearEntityCacheAsync(TId id);
    Task ClearUserRelatedCachesAsync(Guid userId, string entityTypeName);
}