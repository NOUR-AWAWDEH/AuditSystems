using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Contract.Models.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AuditSystem.BusinessLogic.Extensions;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Application.Constants;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class UnifiedCacheService<TId, TEntity, TModel>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<UnifiedCacheService<TId, TEntity, TModel>> logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags)
    : IUnifiedCacheService<TId, TEntity, TModel>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<UnifiedCacheService<TId, TEntity, TModel>> _logger = logger;
    private readonly string _cacheKeyPrefix = cacheKeyPrefix;
    private readonly string[] _entityTags = entityTags;
    private readonly string[] _listTags = listTags;

    public async Task<PaginationResult<TTargetModel>?> GetFromCacheAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page)
    {
        var cachedEntities = await TryGetCachedSearchResults(cacheKey);
        if (cachedEntities == null)
        {
            return null;
        }

        var cachedCount = await TryGetCachedSearchCountAsync(countCacheKey);
        if (!cachedCount.HasValue)
        {
            return null;
        }

        return new PaginationResult<TTargetModel>
        {
            Items = mapper.Map<List<TTargetModel>>(cachedEntities),
            TotalCount = cachedCount.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    public async Task<PaginationResult<TTargetModel>> GetFromRepositoryAsync<TTargetModel>(
            string cacheKey,
            string countCacheKey,
            Page page,
            string? name)
    {
        var predicate = BuildSearchPredicate<TEntity>("Name", name);

        var query = repository.GetAll()
            .Where(predicate)
            .OrderBy(x => x.Id);

        var modelQuery = query.ProjectTo<TTargetModel>(mapper.ConfigurationProvider);
        var result = await modelQuery.PaginateAsync(page, repository);

        var entities = mapper.Map<List<TEntity>>(result.Items);
        await CacheSearchResultsAsync(cacheKey, entities);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return new PaginationResult<TTargetModel>
        {
            Items = result.Items,
            TotalCount = result.TotalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    public static Expression<Func<T, bool>> BuildSearchPredicate<T>(string propertyName, string? searchTerm)
    {
        var parameter = Expression.Parameter(typeof(T), "entity");
        var property = Expression.Property(parameter, propertyName);

        if (property.Type != typeof(string))
            throw new InvalidOperationException($"Property '{propertyName}' is not of type string.");

        if (string.IsNullOrEmpty(searchTerm))
            return Expression.Lambda<Func<T, bool>>(Expression.Constant(true), parameter);

        // Build: entity.PropertyName.ToLower().Contains(searchTerm.ToLower())

        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;

        // entity.PropertyName.ToLower()
        var propertyToLower = Expression.Call(property, toLowerMethod);

        // searchTerm.ToLower()
        var searchTermToLower = Expression.Constant(searchTerm.ToLower(), typeof(string));

        // entity.PropertyName.ToLower().Contains(searchTerm.ToLower())
        var containsCall = Expression.Call(propertyToLower, containsMethod, searchTermToLower);

        return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
    }

    public (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page)
    {
        var cacheKey = GetSearchCacheKey(searchTerm, page);
        var countCacheKey = GetSearchCountCacheKey(searchTerm);
        return (cacheKey, countCacheKey);
    }

    public string GetCacheKey(TId id, string lang = "en") =>
        string.Format(CacheKeys.CacheKey, _cacheKeyPrefix, id, lang);

    public async Task SetCacheAsync(string cacheKey, TEntity entity)
    {
        await _cacheService.SetAsync(
            key: cacheKey,
            value: entity,
            tags: _entityTags,
            expiration: CacheExpirations.MediumTerm);

        await _cacheService.RemoveCacheByTagAsync(_listTags);
    }

    public async Task ClearCacheAsync(string cacheKey)
    {
        await _cacheService.RemoveAsync(cacheKey);
        await _cacheService.RemoveCacheByTagAsync(_listTags);
    }

    public string GetSearchCountCacheKey(string? searchTerm) =>
        string.Format(CacheKeys.SearchCountCacheKey,
            _cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all");

    public string GetSearchCacheKey(string? searchTerm, Page page) =>
        string.Format(CacheKeys.SearchCacheKey,
            _cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all",
            page.Number,
            page.Size);

    public async Task<int?> TryGetCachedSearchCountAsync(string countCacheKey)
    {
        return await _cacheService.GetAsync<int?>(countCacheKey);
    }

    public async Task CacheSearchCountAsync(string countCacheKey, int count)
    {
        await _cacheService.SetAsync(
            key: countCacheKey,
            value: count,
            tags: _listTags,
            expiration: CacheExpirations.MediumTerm);
    }

    public async Task<List<TEntity>?> TryGetCachedSearchResults(string cacheKey)
    {
        var cachedResults = await _cacheService.GetAsync<List<TEntity>>(cacheKey);
        if (cachedResults != null)
            _logger.LogDebug("Retrieved {Count} {EntityType} from cache with key {CacheKey}",
                cachedResults.Count, typeof(TEntity).Name, cacheKey);
        
        return cachedResults;
    }

    public async Task CacheSearchResultsAsync(string cacheKey, IEnumerable<TEntity> entities)
    {
        await _cacheService.SetAsync(
            key: cacheKey,
            value: entities.ToList(),
            tags: _listTags,
            expiration: CacheExpirations.MediumTerm);
    }

    public async Task<TTargetModel?> TryGetFromCacheWithValidationAsync<TTargetModel>(TId id, Guid userId, Expression<Func<TEntity, Guid>> userIdSelector, string unauthorizedMessage)
    {
        var cacheKey = GetCacheKey(id);
        var cachedEntity = await _cacheService.GetAsync<TEntity>(cacheKey);

        if (cachedEntity == null)
        {
            return default;
        }

        // Compile the expression to get the userId from the entity
        var compiledSelector = userIdSelector.Compile();
        var entityUserId = compiledSelector(cachedEntity);

        if (entityUserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(unauthorizedMessage, userId, id));
        }

        return mapper.Map<TTargetModel>(cachedEntity);
    }

    public async Task CacheEntityAsync(TEntity entity)
    {
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    public async Task ClearEntityCacheAsync(TId id)
    {
        var cacheKey = GetCacheKey(id);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task ClearUserRelatedCachesAsync(Guid userId, string entityTypeName)
    {
        // Clear search caches for this user
        var searchPattern = $"{entityTypeName.ToLower()}_search_user_{userId}_*";
        await _cacheService.RemoveAsync(searchPattern);
        
        // Clear user entity list cache
        var userListKey = $"{entityTypeName.ToLower()}_list_user_{userId}";
        await _cacheService.RemoveAsync(userListKey);
        
        _logger.LogDebug("Cleared user-related caches for user {UserId} and entity type {EntityType}", userId, entityTypeName);
    }

    public async Task<PaginationResult<TTargetModel>> GetAllFromRepositoryAsync<TTargetModel>(
    string cacheKey,
    string countCacheKey,
    Page page,
    string? name = null) // keep the parameter but ignore it
    {
        var query = repository.GetAll()
                              .OrderBy(x => x.Id); // just order by Id

        var modelQuery = query.ProjectTo<TTargetModel>(mapper.ConfigurationProvider);
        var result = await modelQuery.PaginateAsync(page, repository);

        // Cache results
        var entities = mapper.Map<List<TEntity>>(result.Items);
        await CacheSearchResultsAsync(cacheKey, entities);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return new PaginationResult<TTargetModel>
        {
            Items = result.Items,
            TotalCount = result.TotalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

}