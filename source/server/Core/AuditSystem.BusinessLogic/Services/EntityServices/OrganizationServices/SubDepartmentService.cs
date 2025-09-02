using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;
using Microsoft.EntityFrameworkCore;


namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubDepartmentService(
    IRepository<Guid, SubDepartment> repository,
    IMapper mapper,
    ICacheService cacheService,
    ISubDepartmentValidation validationHelper,
    ISubDepartmentSearch searchHelper,
    ILogger<SubDepartmentService> logger) : 
    UnifiedCacheService<Guid, SubDepartment, SubDepartmentModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SubDepartment,
        SubDepartmentConstants.SubDepartmentTags,
        SubDepartmentConstants.ListTags
    ),
    ISubDepartmentService
{
    private  readonly ISubDepartmentValidation _validationHelper = validationHelper;
    private  readonly ISubDepartmentSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));
        SubDepartmentValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Creating subdepartment {SubDepartmentName} for user {UserId}",subDepartmentModel.Name, userId);

        await _validationHelper.ValidateDepartmentOwnershipAsync(subDepartmentModel.DepartmentId, userId);

        var entity = mapper.Map<SubDepartment>(subDepartmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created subdepartment {SubDepartmentId} for user {UserId}",
            createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id, Guid userId)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving subdepartment {SubDepartmentId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<SubDepartmentModel>(
            id,
            userId,
            entity => entity.Department.SubLocation.Location.UserId,
            SubDepartmentConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved subdepartment {SubDepartmentId} from cache", id);

            return cachedResult;
        }

        var entity = await _validationHelper.ValidateSubDepartmentOwnershipAsync(id, userId);

        await CacheEntityAsync(entity);

        return mapper.Map<SubDepartmentModel>(entity);
    }

    public async Task DeleteSubDepartmentAsync(Guid subDepartmentId, Guid userId)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Deleting subdepartment {SubDepartmentId} for user {UserId}", subDepartmentId, userId);

        await _validationHelper.ValidateSubDepartmentOwnershipAsync(subDepartmentId, userId);

        await repository.DeleteAsync(subDepartmentId);

        await ClearEntityCacheAsync(subDepartmentId);

        await ClearUserRelatedCachesAsync(userId, SubDepartmentConstants.EntityName);

        logger.LogInformation("Successfully deleted subdepartment {SubDepartmentId}", subDepartmentId);
    }

    public async Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));
        SubDepartmentValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Updating subdepartment {SubDepartmentId} for user {UserId}",
            subDepartmentModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateSubDepartmentOwnershipAsync(subDepartmentModel.Id, userId);

        mapper.Map(subDepartmentModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated subdepartment {SubDepartmentId}", subDepartmentModel.Id);
    }

    public async Task<IReadOnlyList<SubDepartmentModel>> GetSubDepartmentsByUserIdAsync(Guid userId)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all subdepartments for user {UserId}", userId);

        // Create the LINQ query equivalent to your SQL
        return await repository.GetAll()
            .AsNoTracking()
            .Include(s => s.Department) // Include the Department navigation property
            .Where(s => s.Department.SubLocation.Location.UserId == userId)
            .Select(s => new SubDepartmentModel
            {
                Id = s.Id,
                Name = s.Name,
                DepartmentId = s.DepartmentId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<PaginationResult<SubDepartmentModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid departmentId, Page? page)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching subdepartments for user {UserId} with term: {SearchTerm} and departmentId: {DepartmentId}",
            userId, searchTerm ?? "(none)", departmentId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, departmentId, page);

        var cachedResult = await GetFromCacheAsync<SubDepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(SubDepartmentConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, departmentId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<SubDepartmentModel>> SearchAsync(string? name, Page? page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching subdepartments with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SubDepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(SubDepartmentConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<SubDepartmentModel>(cacheKey, countCacheKey, page, name);
    }
}
