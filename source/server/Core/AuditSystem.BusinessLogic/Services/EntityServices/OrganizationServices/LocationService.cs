using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
        LocationConstants.LocationTags,
        LocationConstants.ListTags
    ),
    ILocationService
{
    private readonly LocationValidationHelper _validationHelper = new(repository);
    private readonly LocationSearchHelper _searchHelper = new(repository, mapper, cacheService);

    public async Task<Guid> CreateLocationAsync(LocationModel locationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));
        LocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Creating location for user {UserId}", userId);

        try
        {
            var entity = mapper.Map<Location>(locationModel);
            entity.UserId = userId; // Ensure the userId is set on the entity
            var createdEntity = await repository.CreateAsync(entity);

            await CacheEntityAsync(createdEntity);

            logger.LogInformation("Successfully created location {LocationId} for user {UserId}", 
                createdEntity.Id, userId);
            
            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create location for user {UserId}", userId);
            throw;
        }
    }

    public async Task<LocationModel> GetLocationByIdAsync(Guid id, Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Retrieving location {LocationId} for user {UserId}", id, userId);

        try
        {
            var cachedResult = await TryGetFromCacheWithValidationAsync(id, userId);
            if (cachedResult != null)
            {
                logger.LogDebug("Retrieved location {LocationId} from cache", id);
                return cachedResult;
            }

            var entity = await _validationHelper.ValidateLocationOwnershipAsync(id, userId);
            
            await CacheEntityAsync(entity);
            
            return mapper.Map<LocationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve location {LocationId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task DeleteLocationAsync(Guid locationId, Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Deleting location {LocationId} for user {UserId}", locationId, userId);

        try
        {
            await _validationHelper.ValidateLocationOwnershipAsync(locationId, userId);
            
            await repository.DeleteAsync(locationId);
            
            await ClearEntityCacheAsync(locationId);
            
            logger.LogInformation("Successfully deleted location {LocationId}", locationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete location {LocationId} for user {UserId}", 
                locationId, userId);
            throw;
        }
    }

    public async Task UpdateLocationAsync(LocationModel locationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));
        LocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Updating location {LocationId} for user {UserId}", 
            locationModel.Id, userId);
    
        try
        {
            // Get the tracked entity from validation
            var existingEntity = await _validationHelper.ValidateLocationOwnershipAsync(locationModel.Id, userId);
        
            // Map the model to the existing tracked entity
            mapper.Map(locationModel, existingEntity);
            
            await repository.UpdateAsync(existingEntity);
        
            await CacheEntityAsync(existingEntity);
            
            logger.LogInformation("Successfully updated location {LocationId}", locationModel.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update location {LocationId} for user {UserId}", 
                locationModel.Id, userId);
            throw;
        }
    }

    public async Task<PaginationResult<LocationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        logger.LogDebug("Searching locations with name filter: {Name}", name ?? "(none)");
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<LocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved search results from cache");
            return cachedResult;
        }

        return await GetFromRepositoryAsync<LocationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<LocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page? page)
    {
        LocationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        logger.LogDebug("Searching locations for user {UserId} with term: {SearchTerm}", 
            userId, searchTerm ?? "(none)");
        
        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, page);
    
        var cachedResult = await GetFromCacheAsync<LocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved user search results from cache");
            return cachedResult;
        }
    
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task<ICollection<LocationModel>> GetLocationsByUserIdAsync(Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Retrieving all locations for user {UserId}", userId);

        try
        {
            var query = repository.GetAll()
                .Where(l => l.UserId == userId)
                .Select(l => new LocationModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    UserId = l.UserId,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    Address = l.Address,
                    CompanyId = l.CompanyId,
                });

            var locations = await query.ToListAsync();
            logger.LogDebug("Retrieved {Count} locations for user {UserId}", locations.Count, userId);
            
            return locations;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve locations for user {UserId}", userId);
            throw;
        }
    }

    // Private helper methods
    private async Task<LocationModel?> TryGetFromCacheWithValidationAsync(Guid id, Guid userId)
    {
        var cacheKey = GetCacheKey(id);
        var cachedEntity = await cacheService.GetAsync<Location>(cacheKey);
        
        if (cachedEntity?.UserId == userId)
        {
            return mapper.Map<LocationModel>(cachedEntity);
        }
        
        if (cachedEntity != null && cachedEntity.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(LocationConstants.UnauthorizedAccessMessage, userId, id));
        }
        
        return null;
    }

    private async Task CacheEntityAsync(Location entity)
    {
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    private async Task ClearEntityCacheAsync(Guid entityId)
    {
        var cacheKey = GetCacheKey(entityId);
        await ClearCacheAsync(cacheKey);
    }
}