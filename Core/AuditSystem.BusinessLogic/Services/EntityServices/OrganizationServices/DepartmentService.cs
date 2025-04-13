using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

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