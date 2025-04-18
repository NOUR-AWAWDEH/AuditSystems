using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class DepartmentService(
    IRepository<Guid, Department> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IDepartmentService
{
    private static readonly string[] DepartmentTags = ["departments", "department-list"];
    private static readonly string[] ListTags = ["department-list"]; // Tags for collections only

    public async Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        try
        {
            var entity = mapper.Map<Department>(departmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Department, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: DepartmentTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Department.", ex);
        }
    }
    public async Task DeleteDepartmentAsync(Guid departmentId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(departmentId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Department with ID {departmentId} not found.");
            }
            
            await repository.DeleteAsync(departmentId);
            
            var cacheKey = string.Format(CacheKeys.Department, departmentId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Department.", ex);
        }
    }
    public async Task<DepartmentModel> GetDepartmentByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.Department, id);

            var cachedDepartment = await cacheService.GetAsync<Department>(cacheKey);
            if (cachedDepartment != null)
            {
                return mapper.Map<DepartmentModel>(cachedDepartment);
            }

            var department = await repository.GetByIdAsync(id);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: department,
                tags: DepartmentTags,
                expiration: CacheExpirations.MediumTerm 
            );

            return mapper.Map<DepartmentModel>(department);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get Department ID {id}.", ex);
        }
    }
    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        try
        {
            var entity = mapper.Map<Department>(departmentModel);
            
            await repository.UpdateAsync(entity);
            var cacheKey = string.Format(CacheKeys.Department, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: DepartmentTags,
                expiration: CacheExpirations.MediumTerm);
        
            await cacheService.RemoveCacheByTagAsync(ListTags);
        
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Department.", ex);
        }
    }
}