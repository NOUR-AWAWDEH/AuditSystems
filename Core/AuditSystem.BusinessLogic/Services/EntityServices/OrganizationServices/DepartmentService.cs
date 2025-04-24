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

        var entity = mapper.Map<Department>(departmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
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

    public Task<PaginationResult<DepartmentModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateDepartmentAsync(DepartmentModel departmentModel)
    {
        ArgumentNullException.ThrowIfNull(departmentModel, nameof(departmentModel));

        var entity = mapper.Map<Department>(departmentModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}