using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class DepartmentService(
    IRepository<Guid, Department> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<DepartmentService> logger) : 
    BaseEntityCacheService<Department>(
        cacheService,
        logger,
        CacheKeys.Department,
        DepartmentTags,
        ListTags
    ),
    IDepartmentService
{
    private static readonly string[] DepartmentTags = ["departments", "department-list"];
    private static readonly string[] ListTags = ["department-list"];

    public async Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        try
        {
            var entity = mapper.Map<Department>(departmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for DepartmentModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Department");
            throw new InvalidOperationException("Failed to create Department.", ex);
        }
    }
    
    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        var cacheKey = GetCacheKey(departmentId);
        
        try
        {
            var entity = await repository.GetByIdAsync(departmentId)
                ?? throw new KeyNotFoundException($"Department with ID {departmentId} not found.");

            await repository.DeleteAsync(departmentId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Department with ID {DepartmentId} not found", departmentId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Department ID {DepartmentId}", departmentId);
            throw new InvalidOperationException($"Failed to delete Department: {departmentId}", ex);
        }
    }
    
    public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<Department>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<DepartmentModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Department with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<DepartmentModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Department with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Department with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        try
        {
            var entity = mapper.Map<Department>(departmentModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Department with ID {Id}", departmentModel.Id);
            throw new InvalidOperationException($"Failed to update Department with ID '{departmentModel.Id}'", ex);
        }
    }
}