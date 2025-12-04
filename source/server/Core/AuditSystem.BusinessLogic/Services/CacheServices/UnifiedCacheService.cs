using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;


namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class UnifiedCacheService<TId, TEntity, TModel>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<UnifiedCacheService<TId, TEntity, TModel>> logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags,
    string searchProperty = "Name") :
    EntityCacheHandler<TId, TEntity, TModel>(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags),
    IUnifiedCacheService<TId, TEntity, TModel>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    private readonly SearchCacheHandler<TId, TEntity, TModel> searchHandler =
        new(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags, searchProperty);
    private readonly CacheInvalidationHandler<TId, TEntity, TModel> invalidationHandler =
        new(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags);

    #region Entity Cache

    //Generic version (returns TTargetModel)
    public new Task<TTargetModel?> TryGetFromCacheWithValidationAsync<TTargetModel>(
        TId id,
        Guid userId,
        System.Linq.Expressions.Expression<Func<TEntity, Guid>> userIdSelector,
        string unauthorizedMessage)
        where TTargetModel : class
        => base.TryGetFromCacheWithValidationAsync<TTargetModel>(id, userId, userIdSelector, unauthorizedMessage);

    public new Task CacheEntityAsync(TEntity entity)
        => base.CacheEntityAsync(entity);

    public new Task ClearEntityCacheAsync(TId id)
        => base.ClearEntityCacheAsync(id);

    public new string GetCacheKey(TId id, string lang = "en")
        => base.GetCacheKey(id, lang);

    #endregion

    #region Search Cache

    // Non-generic version (returns TModel)
    public Task<PaginationResult<TModel>?> GetFromCacheAsync(string cacheKey, string countCacheKey, Page page)
        => searchHandler.GetFromCacheAsync(cacheKey, countCacheKey, page);

    // Generic version (returns TTargetModel)
    public Task<PaginationResult<TTargetModel>?> GetFromCacheAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page)
        where TTargetModel : class
        => searchHandler.GetFromCacheAsync<TTargetModel>(cacheKey, countCacheKey, page);

    // Non-generic version (returns TModel)
    public Task<PaginationResult<TModel>> GetFromRepositoryAsync(string cacheKey, string countCacheKey, Page page, string? name = null)
        => searchHandler.GetFromRepositoryAsync(cacheKey, countCacheKey, page, name);

    // Generic version (returns TTargetModel)
    public Task<PaginationResult<TTargetModel>> GetFromRepositoryAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page, string? name = null)
        where TTargetModel : class
        => searchHandler.GetFromRepositoryAsync<TTargetModel>(cacheKey, countCacheKey, page, name);

    public (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page)
        => searchHandler.GetSearchCacheKeys(searchTerm, page);

    #endregion

    #region Cache Invalidation

    public Task InvalidateEntityTagsCacheAsync()
        => invalidationHandler.InvalidateEntityTagsCacheAsync();

    public Task ClearUserRelatedCachesAsync(Guid userId, string entityTypeName, Guid? relatedId = null)
        => invalidationHandler.ClearUserRelatedCachesAsync(userId, entityTypeName, relatedId);

    #endregion
}