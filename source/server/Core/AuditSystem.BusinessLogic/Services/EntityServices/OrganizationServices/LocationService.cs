using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
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
    ICacheService cacheService,
    ILogger<LocationService> _logger,
    ILocationValidation validationHelper,
    ILocationSearch searchHelper,
    IMapper mapper) : 
    UnifiedCacheService<Guid, Location, LocationModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Location,
        LocationConstants.LocationTags,
        LocationConstants.ListTags
    ),
    ILocationService
{
    private readonly ILocationValidation _validationHelper = validationHelper;
    private readonly ILocationSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateLocationAsync(LocationModel locationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));
        LocationValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Creating location for user {UserId}", userId);

        var entity = mapper.Map<Location>(locationModel);
        entity.UserId = userId;
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created location {LocationId} for user {UserId}", 
            createdEntity.Id, userId);
        
        return createdEntity.Id;
    }

    public async Task<LocationModel> GetLocationByIdAsync(Guid id, Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        _logger.LogDebug("Retrieving location {LocationId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<LocationModel>(
            id, 
            userId, 
            entity => entity.UserId, 
            LocationConstants.UnauthorizedAccessMessage);
            
        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved location {LocationId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateLocationOwnershipAsync(id, userId);
        
        await CacheEntityAsync(entity);
        
        return mapper.Map<LocationModel>(entity);
    }

    public async Task DeleteLocationAsync(Guid locationId, Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Deleting location {LocationId} for user {UserId}", locationId, userId);

        await _validationHelper.ValidateLocationOwnershipAsync(locationId, userId);
        
        // Get the location first to retrieve the CompanyId before deletion
        var location = await repository.GetByIdAsync(locationId);
        Guid? companyId = location?.CompanyId;
        
        await repository.DeleteAsync(locationId);
        await ClearEntityCacheAsync(locationId);
        await ClearUserRelatedCachesAsync(userId, LocationConstants.EntityName, companyId);
        
        _logger.LogInformation("Successfully deleted location {LocationId} for user {UserId}", locationId, userId);
    }

    public async Task UpdateLocationAsync(LocationModel locationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(locationModel, nameof(locationModel));
        LocationValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Updating location {LocationId} for user {UserId}", 
            locationModel.Id, userId);

        // Get the tracked entity from validation
        var existingEntity = await _validationHelper.ValidateLocationOwnershipAsync(locationModel.Id, userId);
    
        // Map the model to the existing tracked entity
        mapper.Map(locationModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;
        
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
        
        _logger.LogInformation("Successfully updated location {LocationId}", locationModel.Id);
    }

    public async Task<PaginationResult<LocationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        _logger.LogDebug("Searching locations with name filter: {Name}", name ?? "(none)");
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<LocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(LocationConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<LocationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<LocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page page)
    {
        LocationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        _logger.LogDebug("Searching locations for user {UserId} with term: {SearchTerm}", 
            userId, searchTerm ?? "(none)");
        
        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, page);
    
        var cachedResult = await GetFromCacheAsync<LocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved user search results from cache");
            return cachedResult;
        }
    
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task<IReadOnlyList<LocationModel>> GetLocationsByUserIdAsync(Guid userId)
    {
        LocationValidationHelper.ValidateUserId(userId);
        
        _logger.LogDebug("Retrieving all locations for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
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
            })
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();
    }
}