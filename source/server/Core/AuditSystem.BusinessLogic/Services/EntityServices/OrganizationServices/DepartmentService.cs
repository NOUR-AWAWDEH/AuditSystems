using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class DepartmentService(
    IRepository<Guid, Department> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<DepartmentService> logger) : 
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
    private readonly DepartmentValidationHelper _validationHelper = new(repository);
    private readonly DepartmentSearchHelper _searchHelper = new(repository, mapper, cacheService);

    public async Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));
        DepartmentValidationHelper.ValidateUserId(userId);

        try 
        { 
            await _validationHelper.ValidateSubLocationOwnerShipAsync(departmentModel.SubLocationId, userId);

            var entity = mapper.Map<Department>(departmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            await CacheEntityAsync(createdEntity);

            logger.LogInformation("Successfully created department {DepartmentId} for user {UserId}", createdEntity.Id, userId);

            return createdEntity.Id;
        }
        catch (Exception ex)
        { 
            logger.LogError(ex, "Faild to create Department for user {UserId}",  userId);
            throw;
        }
    }
    
    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        var cacheKey = GetCacheKey(departmentId);
        
        var entity = await repository.GetByIdAsync(departmentId)
            ?? throw new KeyNotFoundException($"Department with ID {departmentId} not found.");

        await repository.DeleteAsync(departmentId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var cachedEntity = await cacheService.GetAsync<Department>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<DepartmentModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Department with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<DepartmentModel>(entity);
    }

    public async Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<DepartmentModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<DepartmentModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        var entity = mapper.Map<Department>(departmentModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    // Private helper methods

    private async Task<DepartmentModel?> TryGetFromCasheWithValidationAsync(Guid id, Guid userId)
    {
        var cacheKey = GetCacheKey(id);
        var cachedEntity = await cacheService.GetAsync<Department>(cacheKey);

        if (cachedEntity?.SubLocation?.Location?.UserId == userId)
        { 
            return mapper.Map<DepartmentModel>(cachedEntity);
        }

        if (cachedEntity != null && cachedEntity.SubLocation.Location.UserId != userId)
        {
            throw new UnauthorizedAccessException(
              string.Format(DepartmentConstants.UnauthorizedAccessMessage, userId, id));
        }

        return null;
    }

    private async Task CacheEntityAsync(Department entity)
    {
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }

    private async Task ClearEntityCacheAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        await ClearCacheAsync(cacheKey);
    }

}