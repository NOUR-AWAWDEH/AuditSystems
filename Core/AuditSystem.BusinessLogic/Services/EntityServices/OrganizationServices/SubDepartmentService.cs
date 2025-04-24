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

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class SubDepartmentService(
    IRepository<Guid, SubDepartment> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubDepartmentService> logger) : 
    UnifiedCacheService<Guid, SubDepartment, SubDepartmentModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SubDepartment,
        SubDepartmentTags,
        ListTags
    ),
    ISubDepartmentService
{
    private static readonly string[] SubDepartmentTags = ["sub-departments", "sub-department-list"];
    private static readonly string[] ListTags = ["sub-department-list"];

    public async Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));

        var entity = mapper.Map<SubDepartment>(subDepartmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteSubDepartmentAsync(Guid subDepartmentId)
    {
        var cacheKey = GetCacheKey(subDepartmentId);
        
        var entity = await repository.GetByIdAsync(subDepartmentId)
            ?? throw new KeyNotFoundException($"SubDepartment with ID {subDepartmentId} not found.");

        await repository.DeleteAsync(subDepartmentId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<SubDepartmentModel> GetSubDepartmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
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

    public Task<PaginationResult<SubDepartmentModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));
        
        var entity = mapper.Map<SubDepartment>(subDepartmentModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
