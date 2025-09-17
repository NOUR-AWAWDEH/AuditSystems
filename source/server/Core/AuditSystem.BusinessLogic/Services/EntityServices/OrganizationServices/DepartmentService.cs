using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;
using Microsoft.EntityFrameworkCore;


namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class DepartmentService(
    IRepository<Guid, Department> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<DepartmentService> logger,
    IDepartmentValidation validationHelper,
    IDepartmentSearch searchHelper) : 
    UnifiedCacheService<Guid, Department, DepartmentModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Department,
        DepartmentConstants.DepartmentTags,
        DepartmentConstants.ListTags
    ),
    IDepartmentService
{
    private readonly IDepartmentValidation _validationHelper = validationHelper;
    private readonly IDepartmentSearch _searchHelper = searchHelper;


    public async Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));
        DepartmentValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Creating department {DepartmentName} for user {UserId}", departmentModel.Name, userId);

        await _validationHelper.ValidateSubLocationOwnershipAsync(departmentModel.SubLocationId, userId);

        var entity = mapper.Map<Department>(departmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created department {DepartmentId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }
    
    public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id, Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving department {DepartmentId} for user {UserId}",id,userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<DepartmentModel>(
            id,
            userId,
            entity => entity.SubLocation.Location.UserId,
            DepartmentConstants.UnauthorizedAccessMessage);

        if (cashedResult != null) 
        {
            logger.LogDebug("Retrieved department {DepartmentId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateDepartmentOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<DepartmentModel>(entity);
    }

    public async Task DeleteDepartmentAsync(Guid departmentId, Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Deleting department {DepartmentId} for user {UserId}", departmentId, userId);

        await _validationHelper.ValidateDepartmentOwnershipAsync(departmentId, userId);
        await repository.DeleteAsync(departmentId);
        await ClearEntityCacheAsync(departmentId);
        await ClearUserRelatedCachesAsync(userId, "Department");

        logger.LogInformation("Successfully deleted department {departmentId}",departmentId);
    }
    
    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));
        DepartmentValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Updating department {DepartmentId} for user {UserId}", departmentModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateDepartmentOwnershipAsync(departmentModel.Id, userId);
        
        mapper.Map(departmentModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated department {DepartmentId}", departmentModel.Id);
    }
    
    public async Task<IReadOnlyList<DepartmentModel>> GetDepartmentsByUserIdAsync(Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retriving all departments for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Include(s => s.SubLocation)
            .Where(r => r.SubLocation.Location.UserId == userId)
            .Select(d => new DepartmentModel
            { 
                Id = d.Id,
                Name = d.Name,
                SubLocationId = d.SubLocationId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .ToListAsync();
    }

    public async Task<PaginationResult<DepartmentModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid subLocationId, Page? page)
    {
        DepartmentValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching departments for user {UserId} with term: {SearchTerm} and sublocation: {SubLocationId}",
            userId, searchTerm ?? "(none)", subLocationId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, subLocationId, page);

        var cachedResult = await GetFromCacheAsync<DepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            logger.LogDebug(DepartmentConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, subLocationId, page, cacheKey, countCacheKey);
    }
    
    public async Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching departments with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<DepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            logger.LogDebug(DepartmentConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<DepartmentModel>(cacheKey, countCacheKey, page, name);
    }
}