using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubLocationService(
    IRepository<Guid, SubLocation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubLocationService> logger) : 
    BaseEntityCacheService<SubLocation>(
        cacheService,
        logger,
        CacheKeys.SubLocation,
        SubLocationTags,
        ListTags
    ),
    ISubLocationService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SubLocationModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SubLocation");
            throw new InvalidOperationException("Failed to create SubLocation.", ex);
        }
    }
    
    public async Task DeleteSubLocationAsync(Guid subLocationId)
    {
        var cacheKey = GetCacheKey(subLocationId);
        
        try
        {
            var entity = await repository.GetByIdAsync(subLocationId)
                ?? throw new KeyNotFoundException($"SubLocation with ID {subLocationId} not found.");

            await repository.DeleteAsync(subLocationId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SubLocation with ID {SubLocationId} not found", subLocationId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SubLocation ID {SubLocationId}", subLocationId);
            throw new InvalidOperationException($"Failed to delete SubLocation: {subLocationId}", ex);
        }
    }
    
    public async Task<SubLocationModel> GetSubLocationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<SubLocation>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SubLocationModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SubLocation with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SubLocationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SubLocation with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SubLocation with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));

        try
        {
            var entity = mapper.Map<SubLocation>(subLocationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SubLocation with ID {Id}", subLocationModel.Id);
            throw new InvalidOperationException($"Failed to update SubLocation with ID '{subLocationModel.Id}'", ex);
        }
    }
}