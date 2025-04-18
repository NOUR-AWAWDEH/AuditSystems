using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class LocationService(
    IRepository<Guid, Location> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ILocationService
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

            var cacheKey = string.Format(CacheKeys.Location, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: LocationTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Location.", ex);
        }
    }
    public async Task DeleteLocationAsync(Guid locationId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(locationId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Location with ID {locationId} not found.");
            }
            
            await repository.DeleteAsync(locationId);
            
            var cacheKey = string.Format(CacheKeys.Location, locationId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Location.", ex);
        }
    }
    public async Task<LocationModel> GetLocationByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.Location, id);

            var cachedLocation = await cacheService.GetAsync<Location>(cacheKey);
            if (cachedLocation != null)
            {
                return mapper.Map<LocationModel>(cachedLocation);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Location with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: LocationTags,
                expiration: CacheExpirations.MediumTerm
            ); 

            return mapper.Map<LocationModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get Location by ID {id}.", ex);
        }  
    }
    public async Task UpdateLocationAsync(LocationModel locationModel)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));

        try
        {
            var entity = mapper.Map<Location>(locationModel);
            
            await repository.UpdateAsync(entity);
            var cacheKey = string.Format(CacheKeys.Location, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: LocationTags,
                expiration: CacheExpirations.MediumTerm);
        
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Location.", ex);
        }
    }
}