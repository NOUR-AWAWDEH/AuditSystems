using AutoMapper;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using Microsoft.Extensions.Logging;
using AuditSystem.Application.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using AuditSystem.BusinessLogic.Extensions;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class SearchCacheHandler<TId, TEntity, TModel>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger logger,
    string cacheKeyPrefix,
    string[] entityTags,
    string[] listTags,
    string searchProperty = "Name") : EntityCacheHandler<TId, TEntity, TModel>(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags)
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    private readonly string _searchProperty = searchProperty;

    // Non-generic version (returns TModel)
    public async Task<PaginationResult<TModel>?> GetFromCacheAsync(string cacheKey, string countCacheKey, Page page)
    {
        var cached = await cacheService.GetAsync<List<TModel>>(cacheKey); // Cache TModel, not TEntity
        var count = await cacheService.GetAsync<int?>(countCacheKey);

        if (cached == null || !count.HasValue) return null;

        return new PaginationResult<TModel>
        {
            Items = cached, // Already TModel, no mapping needed
            TotalCount = count.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    // Generic version (returns TTargetModel)
    public async Task<PaginationResult<TTargetModel>?> GetFromCacheAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page)
        where TTargetModel : class
    {
        var cached = await cacheService.GetAsync<List<TTargetModel>>(cacheKey); // Cache TTargetModel
        var count = await cacheService.GetAsync<int?>(countCacheKey);

        if (cached == null || !count.HasValue) return null;

        return new PaginationResult<TTargetModel>
        {
            Items = cached,
            TotalCount = count.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    // Non-generic version (returns TModel)
    public async Task<PaginationResult<TModel>> GetFromRepositoryAsync(
        string cacheKey,
        string countCacheKey,
        Page page,
        string? searchTerm = null,
        Expression<Func<TEntity, bool>>? additionalPredicate = null)
    {
        var predicate = BuildSearchPredicate(searchTerm, additionalPredicate);

        var query = repository.GetAll()
                              .AsNoTracking()
                              .Where(predicate)
                              .OrderBy(e => e.Id);

        var projected = query.ProjectTo<TModel>(mapper.ConfigurationProvider);
        var result = await projected.PaginateAsync(page, repository);

        // Cache the projected models, not the entities
        await CacheSearchResultsAsync(cacheKey, result.Items);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return result;
    }

    // Generic version (returns TTargetModel)
    public async Task<PaginationResult<TTargetModel>> GetFromRepositoryAsync<TTargetModel>(
        string cacheKey,
        string countCacheKey,
        Page page,
        string? searchTerm = null,
        Expression<Func<TEntity, bool>>? additionalPredicate = null)
        where TTargetModel : class
    {
        var predicate = BuildSearchPredicate(searchTerm, additionalPredicate);

        var query = repository.GetAll()
                              .AsNoTracking()
                              .Where(predicate)
                              .OrderBy(e => e.Id);

        var projected = query.ProjectTo<TTargetModel>(mapper.ConfigurationProvider);
        var result = await projected.PaginateAsync(page, repository);

        // Cache the projected models, not the entities
        await CacheSearchResultsAsync(cacheKey, result.Items);
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return result;
    }

    // Cache TModel (non-generic)
    public async Task CacheSearchResultsAsync(string cacheKey, IEnumerable<TModel> models)
    {
        await cacheService.SetAsync(cacheKey, models.ToList(), tags: listTags, expiration: CacheExpirations.MediumTerm);
    }

    // Cache TTargetModel (generic)
    public async Task CacheSearchResultsAsync<TTargetModel>(string cacheKey, IEnumerable<TTargetModel> models)
        where TTargetModel : class
    {
        await cacheService.SetAsync(cacheKey, models.ToList(), tags: listTags, expiration: CacheExpirations.MediumTerm);
    }

    public async Task CacheSearchCountAsync(string countCacheKey, int count)
    {
        await cacheService.SetAsync(countCacheKey, count, tags: listTags, expiration: CacheExpirations.MediumTerm);
    }

    public async Task<int?> TryGetCachedSearchCount(string countCacheKey)
    {
        return await cacheService.GetAsync<int?>(countCacheKey);
    }

    public string GetSearchCacheKey(string? searchTerm, Page page) =>
        string.Format(CacheKeys.SearchCacheKey,
            cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all",
            page.Number,
            page.Size);

    public string GetSearchCountCacheKey(string? searchTerm) =>
        string.Format(CacheKeys.SearchCountCacheKey,
            cacheKeyPrefix,
            searchTerm?.Trim().ToLowerInvariant() ?? "all");

    public (string CacheKey, string CountCacheKey) GetSearchCacheKeys(string? searchTerm, Page page) =>
        (GetSearchCacheKey(searchTerm, page), GetSearchCountCacheKey(searchTerm));

    private Expression<Func<TEntity, bool>> BuildSearchPredicate(
        string? searchTerm,
        Expression<Func<TEntity, bool>>? additionalPredicate = null)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) && additionalPredicate == null)
            return e => true;

        if (string.IsNullOrWhiteSpace(searchTerm))
            return additionalPredicate!;

        var searchPredicate = BuildPropertySearchPredicate(searchTerm);

        if (additionalPredicate == null)
            return searchPredicate;

        // Combine search predicate with additional predicate
        var param = Expression.Parameter(typeof(TEntity), "e");
        var combined = Expression.AndAlso(
            Expression.Invoke(searchPredicate, param),
            Expression.Invoke(additionalPredicate, param));

        return Expression.Lambda<Func<TEntity, bool>>(combined, param);
    }

    private Expression<Func<TEntity, bool>> BuildPropertySearchPredicate(string searchTerm)
    {
        var param = Expression.Parameter(typeof(TEntity), "e");

        // Check if the search property exists on TEntity
        var property = typeof(TEntity).GetProperty(_searchProperty);
        if (property == null)
            throw new InvalidOperationException($"Property '{_searchProperty}' not found on {typeof(TEntity).Name}");

        var prop = Expression.Property(param, _searchProperty);

        if (property.PropertyType != typeof(string))
            throw new InvalidOperationException($"Property '{_searchProperty}' is not of type string.");

        var toLower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;

        var loweredProp = Expression.Call(prop, toLower);
        var loweredTerm = Expression.Constant(searchTerm.ToLower(), typeof(string));
        var containsCall = Expression.Call(loweredProp, contains, loweredTerm);

        return Expression.Lambda<Func<TEntity, bool>>(containsCall, param);
    }

    // Helper method for common search pattern
    public async Task<PaginationResult<TModel>> SearchAsync(
        Page page,
        string? searchTerm = null,
        Expression<Func<TEntity, bool>>? additionalPredicate = null)
    {
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(searchTerm, page);

        var cachedResult = await GetFromCacheAsync(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync(cacheKey, countCacheKey, page, searchTerm, additionalPredicate);
    }

    // Generic version
    public async Task<PaginationResult<TTargetModel>> SearchAsync<TTargetModel>(
        Page page,
        string? searchTerm = null,
        Expression<Func<TEntity, bool>>? additionalPredicate = null)
        where TTargetModel : class
    {
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(searchTerm, page);

        var cachedResult = await GetFromCacheAsync<TTargetModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<TTargetModel>(cacheKey, countCacheKey, page, searchTerm, additionalPredicate);
    }
}