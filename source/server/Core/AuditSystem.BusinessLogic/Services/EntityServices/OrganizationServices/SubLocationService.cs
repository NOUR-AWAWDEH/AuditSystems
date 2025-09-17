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
    private readonly ISubLocationValidation _validationHelper = validationHelper;
    private readonly ISubLocationSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateSubLocationAsync(SubLocationModel subLocationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        SubLocationValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Creating sublocation {SublocationName} for user {UserId}",subLocationModel.Name, userId);

        await _validationHelper.ValidateLocationOwnershipAsync(subLocationModel.LocationId, userId);

        var entity = mapper.Map<SubLocation>(subLocationModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created sublocation {SubLocationId} for user {UserId}",
            createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<SubLocationModel> GetSubLocationByIdAsync(Guid id, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving sublocation {SubLocationId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<SubLocationModel>(
            id,
            userId,
            entity => entity.Location.UserId,
            SubLocationConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved sublocation {SubLocationId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateSubLocationOwnershipAsync(id, userId);

        await CacheEntityAsync(entity);

        return mapper.Map<SubLocationModel>(entity);
    }

    public async Task DeleteSubLocationAsync(Guid subLocationId, Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Deleting sublocation {SubLocationId} for user {UserId}", subLocationId, userId);

        await _validationHelper.ValidateSubLocationOwnershipAsync(subLocationId, userId);

        await repository.DeleteAsync(subLocationId);

        await ClearEntityCacheAsync(subLocationId);

        await ClearUserRelatedCachesAsync(userId, SubLocationConstants.EntityName);

        logger.LogInformation("Successfully deleted sublocation {SubLocationId}", subLocationId);
    }

    public async Task UpdateSubLocationAsync(SubLocationModel subLocationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subLocationModel, nameof(subLocationModel));
        SubLocationValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Updating sublocation {SubLocationId} for user {UserId}",
            subLocationModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateSubLocationOwnershipAsync(subLocationModel.Id, userId);

        mapper.Map(subLocationModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated sublocation {SubLocationId}", subLocationModel.Id);
    }

    public async Task<IReadOnlyList<SubLocationModel>> GetSubLocationsByUserIdAsync(Guid userId)
    {
        SubLocationValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all sublocations for user {UserId}", userId);

        // Create the LINQ query equivalent to your SQL
        return await repository.GetAll()
            .AsNoTracking()
            .Include(s => s.Location)
            .Where(s => s.Location.UserId == userId)
            .Select(s => new SubLocationModel
            {
                Id = s.Id,
                Name = s.Name,
                LocationId = s.LocationId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Address = s.Address
            })
            .ToListAsync();
    }

    public async Task<PaginationResult<SubLocationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid locationId, Page? page)
    {
        SubLocationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching sublocations for user {UserId} with term: {SearchTerm} and locationId: {LocationId}",
            userId, searchTerm ?? "(none)", locationId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, locationId, page);

        var cachedResult = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(SubLocationConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, locationId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<SubLocationModel>> SearchAsync(string? name, Page? page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching sublocations with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SubLocationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(SubLocationConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<SubLocationModel>(cacheKey, countCacheKey, page, name);
    }
}