using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using System.Linq.Expressions;

namespace AuditSystem.Contract.Interfaces.Cache;

public interface IUnifiedCacheService<TId, TEntity, TModel>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    #region Single Entity Cache

    //Generic version (returns TTargetModel)
    Task<TTargetModel?> TryGetFromCacheWithValidationAsync<TTargetModel>(
        TId id,
        Guid userId,
        Expression<Func<TEntity, Guid>> userIdSelector,
        string unauthorizedMessage)
        where TTargetModel : class;

    Task CacheEntityAsync(TEntity entity);
    Task ClearEntityCacheAsync(TId id);
    string GetCacheKey(TId id, string lang = "en");

    #endregion

    #region Search / Repository Cache

    Task<PaginationResult<TModel>?> GetFromCacheAsync(
        string cacheKey,
        string countCacheKey,
        Page page);

    Task<PaginationResult<TModel>> GetFromRepositoryAsync(
        string cacheKey,
        string countCacheKey,
        Page page,
        string? name = null);

    (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page);

    #endregion

    #region Cache Invalidation

    Task InvalidateEntityTagsCacheAsync();
    Task ClearUserRelatedCachesAsync(Guid userId, string entityTypeName, Guid? relatedId = null);

    #endregion
}
