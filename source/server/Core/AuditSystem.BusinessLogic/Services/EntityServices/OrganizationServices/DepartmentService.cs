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
    ILogger<DepartmentService> _logger,
    IDepartmentValidation validationHelper,
    IDepartmentSearch searchHelper) : 
    UnifiedCacheService<Guid, Department, DepartmentModel>(
        repository,
        mapper,
        cacheService,
        _logger,
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

        _logger.LogInformation("Creating department {DepartmentName} for user {UserId}", departmentModel.Name, userId);

        await _validationHelper.ValidateLocationOwnershipAsync(departmentModel.LocationId, userId);

        var entity = mapper.Map<Department>(departmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created department {DepartmentId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }
    
    public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id, Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving department {DepartmentId} for user {UserId}",id,userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<DepartmentModel>(
            id,
            userId,
            DepartmentConstants.DepartmentOwnerIdSelector,
            DepartmentConstants.UnauthorizedAccessMessage);

        if (cashedResult != null) 
        {
            _logger.LogDebug("Retrieved department {DepartmentId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateDepartmentOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<DepartmentModel>(entity);
    }

    public async Task DeleteDepartmentAsync(Guid departmentId, Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Deleting department {DepartmentId} for user {UserId}", departmentId, userId);

        await _validationHelper.ValidateDepartmentOwnershipAsync(departmentId, userId);
        
        var department = await repository.GetByIdAsync(departmentId);
        Guid? locationId = department?.LocationId;
        
        await repository.DeleteAsync(departmentId);
        await ClearEntityCacheAsync(departmentId);
        
        await ClearUserRelatedCachesAsync(userId, DepartmentConstants.EntityName, locationId);

        _logger.LogInformation("Successfully deleted department {DepartmentId} for user {UserId}", departmentId, userId);
    }
    
    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));
        DepartmentValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating department {DepartmentId} for user {UserId}", departmentModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateDepartmentOwnershipAsync(departmentModel.Id, userId);
        
        mapper.Map(departmentModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated department {DepartmentId}", departmentModel.Id);
    }
    
    public async Task<IReadOnlyList<DepartmentModel>> GetDepartmentsByUserIdAsync(Guid userId)
    {
        DepartmentValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all departments for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Where(DepartmentConstants.DepartmentOwnerPredicate(userId))
            .Select(d => new DepartmentModel
            {
                Id = d.Id,
                Name = d.Name,
                LocationId = d.LocationId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<DepartmentModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid locationId, Page? page)
    {
        DepartmentValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching departments for user {UserId} with term: {SearchTerm} and location: {LocationId}",
            userId, searchTerm ?? "(none)", locationId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, locationId, page);

        var cachedResult = await GetFromCacheAsync<DepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(DepartmentConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, locationId, page, cacheKey, countCacheKey);
    }
    
    public async Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching departments with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<DepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(DepartmentConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<DepartmentModel>(cacheKey, countCacheKey, page, name);
    }
}