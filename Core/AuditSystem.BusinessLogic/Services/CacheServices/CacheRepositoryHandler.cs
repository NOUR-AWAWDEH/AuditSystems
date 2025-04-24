using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Contract.Models.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AuditSystem.BusinessLogic.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.CacheServices;

public class CacheRepositoryHandler<TId, TEntity>(
    IRepository<TId, TEntity> repository,
    IMapper mapper,
    IBaseEntityCacheService<TEntity> cacheService)
    : ICacheRepositoryHandler<TId, TEntity>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    public async Task<PaginationResult<TModel>?> GetFromCacheAsync<TModel>(string cacheKey, string countCacheKey, Page page)
    {
        var cachedEntities = await cacheService.TryGetCachedSearchResults(cacheKey);
        if (cachedEntities == null)
        {
            return null;
        }

        var cachedCount = await cacheService.TryGetCachedSearchCountAsync(countCacheKey);
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

    public async Task<PaginationResult<TModel>> GetFromRepositoryAsync<TModel>(string cacheKey, string countCacheKey, Page page, string? name)
    {
        var query = repository.GetAll()
            .Where(entity => string.IsNullOrEmpty(name) || EF.Property<string>(entity, "Name").Contains(name, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Id);

        var modelQuery = query.ProjectTo<TModel>(mapper.ConfigurationProvider);
        var result = await modelQuery.PaginateAsync(page, repository);

        var entities = mapper.Map<List<TEntity>>(result.Items);
        await cacheService.CacheSearchResultsAsync(cacheKey, entities);
        await cacheService.CacheSearchCountAsync(countCacheKey, result.TotalCount);

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
        var cacheKey = cacheService.GetSearchCacheKey(searchTerm, page);
        var countCacheKey = cacheService.GetSearchCountCacheKey(searchTerm);
        return (cacheKey, countCacheKey);
    }
}
