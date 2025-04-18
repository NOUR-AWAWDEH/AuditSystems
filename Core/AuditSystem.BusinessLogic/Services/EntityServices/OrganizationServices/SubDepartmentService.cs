using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubDepartmentService(
    IRepository<Guid, SubDepartment> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubDepartmentService
{
    private static readonly string[] SubDepartmentTags = ["sub-departments", "sub-department-list"];
    private static readonly string[] ListTags = ["sub-department-list"]; // Tags for collections only

    public async Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));

        try
        {
            var entity = mapper.Map<SubDepartment>(subDepartmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SubDepartment, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SubDepartmentTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SubDepartment.", ex);
        }
    }
    public async Task DeleteSubDepartmentAsync(Guid subDepartmentId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(subDepartmentId);
            if (entity == null)
            {
               throw new KeyNotFoundException($"SubDepartment with ID {subDepartmentId} not found.");
            }

            await repository.DeleteAsync(subDepartmentId);

            var cacheKey = string.Format(CacheKeys.SubDepartment, subDepartmentId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete SubDepartment.", ex);
        }
    }
    public async Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.SubDepartment, id);

            var cachedEntity = await cacheService.GetAsync<SubDepartment>(cacheKey);

            if (cachedEntity != null)
            {
                return mapper.Map<SubDepartmentModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"SubDepartment with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SubDepartmentTags,
                expiration: CacheExpirations.MediumTerm  
            );

            return mapper.Map<SubDepartmentModel>(entity);  
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get SubDepartment by ID {id}.", ex);
        }
    }
    public async Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));

        try
        {
            var entity = mapper.Map<SubDepartment>(subDepartmentModel);
            
            await repository.UpdateAsync(entity);
            var cacheKey = string.Format(CacheKeys.SubDepartment, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SubDepartmentTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SubDepartment.", ex);
        }
    }
}
