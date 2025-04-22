using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubDepartmentService(
    IRepository<Guid, SubDepartment> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubDepartmentService> logger) : 
    BaseEntityCacheService<SubDepartment>(
        cacheService,
        logger,
        CacheKeys.SubDepartment,
        SubDepartmentTags,
        ListTags
    ),
    ISubDepartmentService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SubDepartmentModel");
            throw new ArgumentException("Invalid data mapping occurred", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SubDepartment");
            throw new InvalidOperationException("Failed to create SubDepartment.", ex);
        }
    }
    
    public async Task DeleteSubDepartmentAsync(Guid subDepartmentId)
    {
        var cacheKey = GetCacheKey(subDepartmentId);
        
        try
        {
            var entity = await repository.GetByIdAsync(subDepartmentId)
                ?? throw new KeyNotFoundException($"SubDepartment with ID {subDepartmentId} not found.");

            await repository.DeleteAsync(subDepartmentId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SubDepartment with ID {SubDepartmentId} not found", subDepartmentId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SubDepartment ID {SubDepartmentId}", subDepartmentId);
            throw new InvalidOperationException($"Failed to delete SubDepartment: {subDepartmentId}", ex);
        }
    }
    
    public async Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<SubDepartment>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SubDepartmentModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SubDepartment with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SubDepartmentModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SubDepartment with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SubDepartment with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));

        try
        {
            var entity = mapper.Map<SubDepartment>(subDepartmentModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SubDepartment with ID {Id}", subDepartmentModel.Id);
            throw new InvalidOperationException($"Failed to update SubDepartment with ID '{subDepartmentModel.Id}'", ex);
        }
    }
}
