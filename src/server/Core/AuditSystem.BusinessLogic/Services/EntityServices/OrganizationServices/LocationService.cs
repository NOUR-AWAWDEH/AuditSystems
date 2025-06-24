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

internal sealed class LocationService(
    IRepository<Guid, Location> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<LocationService> logger) : 
    UnifiedCacheService<Guid, Location, LocationModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Location,
        LocationTags,
        ListTags
    ),
    ILocationService
{
    private static readonly string[] LocationTags = ["locations", "location-list"];
    private static readonly string[] ListTags = ["location-list"];

    public async Task<Guid> CreateLocationAsync(LocationModel locationModel)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));

        var entity = mapper.Map<Location>(locationModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteLocationAsync(Guid locationId)
    {
        var cacheKey = GetCacheKey(locationId);
       
        var entity = await repository.GetByIdAsync(locationId)
            ?? throw new KeyNotFoundException($"Location with ID {locationId} not found.");

        await repository.DeleteAsync(locationId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<LocationModel> GetLocationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<Location>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<LocationModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Location with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<LocationModel>(entity);
    }

    public async Task<PaginationResult<LocationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<LocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<LocationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateLocationAsync(LocationModel locationModel)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));

        var entity = mapper.Map<Location>(locationModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    public async Task<IEnumerable<LocationModel>> GetLocationsByUserIdAsync(Guid userId)
    {
        var locations = await repository.GetAllAsync(l => l.UserId == userId);
        return mapper.Map<IEnumerable<LocationModel>>(locations);
    }
}