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
    string[] listTags) : EntityCacheHandler<TId, TEntity, TModel>(repository, mapper, cacheService, logger, cacheKeyPrefix, entityTags, listTags)
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{

    // Non-generic version (returns TModel)
    public async Task<PaginationResult<TModel>?> GetFromCacheAsync(string cacheKey, string countCacheKey, Page page)
    {
        var cached = await cacheService.GetAsync<List<TEntity>>(cacheKey);
        var count = await cacheService.GetAsync<int?>(countCacheKey);

        if (cached == null || !count.HasValue) return null;

        return new PaginationResult<TModel>
        {
            Items = mapper.Map<List<TModel>>(cached),
            TotalCount = count.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    // Generic version (returns TTargetModel) - ADD THIS METHOD
    public async Task<PaginationResult<TTargetModel>?> GetFromCacheAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page)
        where TTargetModel : class
    {
        var cached = await cacheService.GetAsync<List<TEntity>>(cacheKey);
        var count = await cacheService.GetAsync<int?>(countCacheKey);

        if (cached == null || !count.HasValue) return null;

        return new PaginationResult<TTargetModel>
        {
            Items = mapper.Map<List<TTargetModel>>(cached),
            TotalCount = count.Value,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }

    // Non-generic version (returns TModel)
    public async Task<PaginationResult<TModel>> GetFromRepositoryAsync(string cacheKey, string countCacheKey, Page page, string? searchTerm = null)
    {
        Expression<Func<TEntity, bool>> predicate = string.IsNullOrWhiteSpace(searchTerm)
            ? e => true
            : BuildSearchPredicate<TEntity>("Name", searchTerm);

        var query = repository.GetAll()
                              .AsNoTracking()
                              .Where(predicate)
                              .OrderBy(e => e.Id);

        var projected = query.ProjectTo<TModel>(mapper.ConfigurationProvider);

        var result = await projected.PaginateAsync(page, repository);

        await CacheSearchResultsAsync(cacheKey, mapper.Map<List<TEntity>>(result.Items));
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return result;
    }

    // Generic version (returns TTargetModel) - ADD THIS METHOD
    public async Task<PaginationResult<TTargetModel>> GetFromRepositoryAsync<TTargetModel>(string cacheKey, string countCacheKey, Page page, string? searchTerm = null)
        where TTargetModel : class
    {
        Expression<Func<TEntity, bool>> predicate = string.IsNullOrWhiteSpace(searchTerm)
            ? e => true
            : BuildSearchPredicate<TEntity>("Name", searchTerm);

        var query = repository.GetAll()
                              .AsNoTracking()
                              .Where(predicate)
                              .OrderBy(e => e.Id);

        var projected = query.ProjectTo<TTargetModel>(mapper.ConfigurationProvider);

        var result = await projected.PaginateAsync(page, repository);

        await CacheSearchResultsAsync(cacheKey, mapper.Map<List<TEntity>>(result.Items));
        await CacheSearchCountAsync(countCacheKey, result.TotalCount);

        return result;
    }

    public async Task CacheSearchResultsAsync(string cacheKey, IEnumerable<TEntity> entities)
    {
        await cacheService.SetAsync(cacheKey, entities.ToList(), tags: listTags, expiration: CacheExpirations.MediumTerm);
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

    private static Expression<Func<T, bool>> BuildSearchPredicate<T>(string propertyName, string? searchTerm)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var prop = Expression.Property(param, propertyName);

        if (prop.Type != typeof(string))
            throw new InvalidOperationException($"Property '{propertyName}' is not of type string.");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return e => true;

        var toLower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;

        var loweredProp = Expression.Call(prop, toLower);
        var loweredTerm = Expression.Constant(searchTerm.ToLower(), typeof(string));
        var containsCall = Expression.Call(loweredProp, contains, loweredTerm);

        return Expression.Lambda<Func<T, bool>>(containsCall, param);
    }
}