using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
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
    ISubLocationValidation validationHelper,
    ISubLocationSearch searchHelper,
    ILogger<SubLocationService> _logger) :
    UnifiedCacheService<Guid, SubLocation, SubLocationModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.SubLocation,
        SubLocationConstants.SubLocationTags,
        SubLocationConstants.ListTags
    ),
    ISubLocationService
{
    private readonly ISubLocationValidation _validationHelper = validationHelper;
    private readonly ISubLocationSearch _searchHelper = searchHelper;

    // CREATE
    public async Task<Guid> CreateSubLocationAsync(SubLocationModel model, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(model);
        SubLocationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating sublocation {SublocationName} for user {UserId}", model.Name, userId);

        await _validationHelper.ValidateLocationOwnershipAsync(model.LocationId, userId);

        var entity = mapper.Map<SubLocation>(model);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created sublocation {SubLocationId} for user {UserId}", createdEntity.Id, userId);
        return createdEntity.Id;
    }

    // GET BY ID
    public async Task<SubLocationModel> GetSubLocationByIdAsync(Guid subLocationId, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving sublocation {SubLocationId} for user {UserId}", subLocationId, userId);

        // Try cache first
        var cachedResult = await TryGetFromCacheWithValidationAsync<SubLocationModel>(
            subLocationId,
            userId,
            entity => entity.Location.UserId,
            SubLocationConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved sublocation {SubLocationId} from cache", subLocationId);
            return cachedResult;
        }

        // Fetch sublocation + location in a single query
        var entity = await repository.Query()
            .Include(sl => sl.Location)
            .FirstOrDefaultAsync(sl => sl.Id == subLocationId) ?? throw new KeyNotFoundException(string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationId));

        // Validate ownership using preloaded entity
        await _validationHelper.ValidateSubLocationOwnershipAsync(entity, userId);

        // Cache & return
        await CacheEntityAsync(entity);
        return mapper.Map<SubLocationModel>(entity);
    }

    // UPDATE
    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        SubLocationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating sublocation {SubLocationId} for user {UserId}",
            subLocationModel.Id, userId);

        // Fetch sublocation + location in a single query
        var existingEntity = await repository.Query()
            .Include(sl => sl.Location)
            .FirstOrDefaultAsync(sl => sl.Id == subLocationModel.Id) ?? throw new KeyNotFoundException(string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationModel.Id));

        // Validate ownership using preloaded entity
        await _validationHelper.ValidateSubLocationOwnershipAsync(existingEntity, userId);

        // Map changes and update timestamp
        mapper.Map(subLocationModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        // Update DB & cache
        await repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated sublocation {SubLocationId}", subLocationModel.Id);
    }


    // DELETE
    public async Task DeleteSubLocationAsync(Guid subLocationId, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Deleting sublocation {SubLocationId} for user {UserId}", subLocationId, userId);

        // Fetch sublocation + location in a single query
        var subLocation = await repository.Query()
            .Include(sl => sl.Location)
            .FirstOrDefaultAsync(sl => sl.Id == subLocationId);

        if (subLocation == null)
            throw new KeyNotFoundException(string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationId));

        // Validate ownership using preloaded entity
        await _validationHelper.ValidateSubLocationOwnershipAsync(subLocation, userId);

        // Delete from DB
        await repository.DeleteAsync(subLocationId);

        // Clear cache
        await ClearEntityCacheAsync(subLocationId);
        await ClearUserRelatedCachesAsync(userId, SubLocationConstants.EntityName, subLocation.Location.Id);

        logger.LogInformation("Successfully deleted sublocation {SubLocationId} for user {UserId}", subLocationId, userId);
    }

    // GET ALL BY USER
    public async Task<IReadOnlyList<SubLocationModel>> GetSubLocationsByUserIdAsync(Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all sublocations for user {UserId}", userId);
        
        var subLocations = await repository.GetAll()
            .AsNoTracking()
            .Include(sl => sl.Location)
            .Where(sl => sl.Location.UserId == userId)
            .Select(sl => new SubLocationModel
            {
                Id = sl.Id,
                Name = sl.Name,
                LocationId = sl.LocationId,
                CreatedAt = sl.CreatedAt,
                UpdatedAt = sl.UpdatedAt,
                Address = sl.Address
            })
            .OrderBy(sl => sl.CreatedAt)
            .ToListAsync();

        return subLocations;
    }

    // SEARCH (User-specific)
    public async Task<PaginationResult<SubLocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid locationId, Page? page)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page);

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, locationId, page);

        var cached = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cached != null)
            return cached;

        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, locationId, page, cacheKey, countCacheKey);

        return result;
    }
}