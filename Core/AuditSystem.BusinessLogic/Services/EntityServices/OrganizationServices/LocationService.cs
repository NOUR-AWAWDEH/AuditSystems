using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class LocationService(
    IRepository<Guid, Location> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<LocationService> logger) : 
    BaseEntityCacheService<Location>(
        cacheService,
        logger,
        CacheKeys.Location,
        LocationTags,
        ListTags
    ),
    ILocationService
{
    private static readonly string[] LocationTags = ["locations", "location-list"];
    private static readonly string[] ListTags = ["location-list"]; // Tags for collections only

    public async Task<Guid> CreateLocationAsync(LocationModel locationModel)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));

        try
        {
            var entity = mapper.Map<Location>(locationModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for LocationModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Location");
            throw new InvalidOperationException("Failed to create Location.", ex);
        }
    }
    
    public async Task DeleteLocationAsync(Guid locationId)
    {
        var cacheKey = GetCacheKey(locationId);
        
        try
        {
            var entity = await repository.GetByIdAsync(locationId)
                ?? throw new KeyNotFoundException($"Location with ID {locationId} not found.");

            await repository.DeleteAsync(locationId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Location with ID {LocationId} not found", locationId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Location ID {LocationId}", locationId);
            throw new InvalidOperationException($"Failed to delete Location: {locationId}", ex);
        }
    }
    
    public async Task<LocationModel> GetLocationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<Location>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<LocationModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Location with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<LocationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Location with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Location with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<LocationModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateLocationAsync(LocationModel locationModel)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));

        try
        {
            var entity = mapper.Map<Location>(locationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Location with ID {Id}", locationModel.Id);
            throw new InvalidOperationException($"Failed to update Location with ID '{locationModel.Id}'", ex);
        }
    }
}