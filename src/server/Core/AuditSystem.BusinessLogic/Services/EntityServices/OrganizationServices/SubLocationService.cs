using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubLocationService(
    IRepository<Guid, SubLocation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubLocationService> logger) : 
    UnifiedCacheService<Guid, SubLocation, SubLocationModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SubLocation,
        SubLocationTags,
        ListTags
    ),
    ISubLocationService
{
    private static readonly string[] SubLocationTags = ["sub-locations", "sub-location-list"];
    private static readonly string[] ListTags = ["sub-location-list"];

    public async Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));

        var entity = mapper.Map<SubLocation>(subLocationModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteSubLocationAsync(Guid subLocationId)
    {
        var cacheKey = GetCacheKey(subLocationId);
        
        var entity = await repository.GetByIdAsync(subLocationId)
            ?? throw new KeyNotFoundException($"SubLocation with ID {subLocationId} not found.");

        await repository.DeleteAsync(subLocationId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<SubLocationModel> GetSubLocationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<SubLocation>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<SubLocationModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"SubLocation with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SubLocationModel>(entity);
    }

    public async Task<PaginationResult<SubLocationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<SubLocationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        
        var entity = mapper.Map<SubLocation>(subLocationModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}