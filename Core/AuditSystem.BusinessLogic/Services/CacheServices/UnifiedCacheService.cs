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

    public async Task<PaginationResult<TModel>?> GetFromCacheAsync(string cacheKey, string countCacheKey, Page page)
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

        var models = mapper.Map<List<TModel>>(cachedEntities);
        return new PaginationResult<TModel>
        {
            Items = models,
            TotalCount = cachedCount.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    public async Task<PaginationResult<TModel>> GetFromRepositoryAsync(string cacheKey, string countCacheKey, Page page, string? searchTerm, string propertyName)
    {
        var predicate = BuildSearchPredicate<TEntity>(propertyName, searchTerm);

        var query = repository.GetAll()
            .Where(predicate)
            .OrderBy(x => x.Id);

        var modelQuery = query.ProjectTo<TModel>(mapper.ConfigurationProvider);
        var result = await modelQuery.PaginateAsync(page, repository);

        var entities = mapper.Map<List<TEntity>>(result.Items);
        await CacheSearchResultsAsync(cacheKey, entities);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return new PaginationResult<TModel>
        {
            Items = result.Items,
            TotalCount = result.TotalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    public static Expression<Func<TEntity, bool>> BuildSearchPredicate<TEntity>(string propertyName, string? searchTerm)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var property = Expression.Property(parameter, propertyName);

        if (property.Type != typeof(string))
            throw new InvalidOperationException($"Property '{propertyName}' is not of type string.");

        if (string.IsNullOrEmpty(searchTerm))
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Constant(true), parameter);

        // Build: entity.PropertyName.ToLower().Contains(searchTerm.ToLower())

        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;

        // entity.PropertyName.ToLower()
        var propertyToLower = Expression.Call(property, toLowerMethod);

        // searchTerm.ToLower()
        var searchTermToLower = Expression.Constant(searchTerm.ToLower(), typeof(string));

        // entity.PropertyName.ToLower().Contains(searchTerm.ToLower())
        var containsCall = Expression.Call(propertyToLower, containsMethod, searchTermToLower);

        return Expression.Lambda<Func<TEntity, bool>>(containsCall, parameter);
    }



    public async Task<PaginationResult<TModel>?> GetFromCacheAsync<TModel>(string cacheKey, string countCacheKey, Page page)
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

        return new PaginationResult<TModel>
        {
            Items = mapper.Map<List<TModel>>(cachedEntities),
            TotalCount = cachedCount.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    public async Task<PaginationResult<TModel>> GetFromRepositoryAsync<TModel>(string cacheKey, string countCacheKey, Page page, string? name)
    {
        var predicate = BuildSearchPredicate<TEntity>("Name", name);

        var query = repository.GetAll()
            .Where(predicate)
            .OrderBy(x => x.Id);

        var modelQuery = query.ProjectTo<TModel>(mapper.ConfigurationProvider);

        var result = await modelQuery.PaginateAsync(page, repository);

        var entities = mapper.Map<List<TEntity>>(result.Items);

        await CacheSearchResultsAsync(cacheKey, entities);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return new PaginationResult<TModel>
        {
            Items = result.Items,
            TotalCount = result.TotalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
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
}