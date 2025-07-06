using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.Sublocation;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;
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
        SubLocationConstants.SubLocationTags,
        SubLocationConstants.ListTags
    ),
    ISubLocationService
{
    private readonly SubLocationValidationHelper _validationHelper = new(repository);
    private readonly SubLocationSearchHelper _searchHelper = new(repository, mapper, cacheService);

    public async Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        SubLocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Creating sublocation for user {UserId}", userId);

        try
        {
            await _validationHelper.ValidateLocationOwnershipAsync(subLocationModel.LocationId, userId);

            var entity = mapper.Map<SubLocation>(subLocationModel);
            var createdEntity = await repository.CreateAsync(entity);

            await CacheEntityAsync(createdEntity);

            logger.LogInformation("Successfully created sublocation {SubLocationId} for user {UserId}", 
                createdEntity.Id, userId);
            
            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create sublocation for user {UserId}", userId);
            throw;
        }
    }

    public async Task<SubLocationModel> GetSubLocationByIdAsync(Guid id, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Retrieving sublocation {SubLocationId} for user {UserId}", id, userId);

        try
        {
            var cachedResult = await TryGetFromCacheWithValidationAsync(id, userId);
            if (cachedResult != null)
            {
                logger.LogDebug("Retrieved sublocation {SubLocationId} from cache", id);
                return cachedResult;
            }

            var entity = await _validationHelper.ValidateSubLocationOwnershipAsync(id, userId);
            
            await CacheEntityAsync(entity);
            
            return mapper.Map<SubLocationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve sublocation {SubLocationId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task DeleteSubLocationAsync(Guid subLocationId, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Deleting sublocation {SubLocationId} for user {UserId}", subLocationId, userId);

        try
        {
            await _validationHelper.ValidateSubLocationOwnershipAsync(subLocationId, userId);
            
            await repository.DeleteAsync(subLocationId);
            
            await ClearEntityCacheAsync(subLocationId);
            
            logger.LogInformation("Successfully deleted sublocation {SubLocationId}", subLocationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete sublocation {SubLocationId} for user {UserId}", 
                subLocationId, userId);
            throw;
        }
    }

    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        SubLocationValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Updating sublocation {SubLocationId} for user {UserId}", 
            subLocationModel.Id, userId);

        try
        {
            var existingEntity = await _validationHelper.ValidateSubLocationOwnershipAsync(subLocationModel.Id, userId);
        
            mapper.Map(subLocationModel, existingEntity);
            await repository.UpdateAsync(existingEntity);
        
            await CacheEntityAsync(existingEntity);
            
            logger.LogInformation("Successfully updated sublocation {SubLocationId}", subLocationModel.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update sublocation {SubLocationId} for user {UserId}", 
                subLocationModel.Id, userId);
            throw;
        }
    }

    public async Task<PaginationResult<SubLocationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        logger.LogDebug("Searching sublocations with name filter: {Name}", name ?? "(none)");
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved search results from cache");
            return cachedResult;
        }

        return await GetFromRepositoryAsync<SubLocationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<SubLocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page? page)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        logger.LogDebug("Searching sublocations for user {UserId} with term: {SearchTerm}", 
            userId, searchTerm ?? "(none)");
        
        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, page);
    
        var cachedResult = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved user search results from cache");
            return cachedResult;
        }
    
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    // Private helper methods
    private async Task<SubLocationModel?> TryGetFromCacheWithValidationAsync(Guid id, Guid userId)
    {
        var cacheKey = GetCacheKey(id);
        var cachedEntity = await cacheService.GetAsync<SubLocation>(cacheKey);
        
        if (cachedEntity?.Location?.UserId == userId)
        {
            return mapper.Map<SubLocationModel>(cachedEntity);
        }
        
        if (cachedEntity != null && cachedEntity.Location?.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, id));
        }
        
        return null;
    }

    private async Task CacheEntityAsync(SubLocation entity)
    {
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    private async Task ClearEntityCacheAsync(Guid entityId)
    {
        var cacheKey = GetCacheKey(entityId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<ICollection<SubLocationModel>> GetSubLocationsByUserIdAsync(Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Retrieving all sublocations for user {UserId}", userId);
    
        try
        {
            // Create the LINQ query equivalent to your SQL
            var query = repository.GetAll()
                .Include(s => s.Location) // Include the Location navigation property
                .Where(s => s.Location.UserId == userId)
                .Select(s => new SubLocationModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    LocationId = s.LocationId,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    Address = s.Address
                });
    
            // Execute the query and get results
            var subLocations = await query.ToListAsync();
            logger.LogDebug("Retrieved {Count} sublocations for user {UserId}", subLocations.Count, userId);
            
            return subLocations;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve sublocations for user {UserId}", userId);
            throw;
        }
    }
}