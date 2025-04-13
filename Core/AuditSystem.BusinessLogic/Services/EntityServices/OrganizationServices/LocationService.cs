using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

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