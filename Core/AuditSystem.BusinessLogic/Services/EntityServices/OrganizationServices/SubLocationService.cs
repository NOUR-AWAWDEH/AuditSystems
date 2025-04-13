using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class SubLocationService(
    IRepository<Guid, SubLocation> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubLocationService
{
    private static readonly string[] SubLocationTags = ["sub-locations", "sub-location-list"];
    private static readonly string[] ListTags = ["sub-location-list"]; // Tags for collections only

    public async Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));

        try
        {
            var entity = mapper.Map<SubLocation>(subLocationModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SubLocation, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SubLocationTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SubLocation.", ex);
        }
    }

    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));

        try
        {
            var entity = mapper.Map<SubLocation>(subLocationModel);
            
            await repository.UpdateAsync(entity);
            var cacheKey = string.Format(CacheKeys.SubLocation, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SubLocationTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SubLocation.", ex);
        }
    }
}