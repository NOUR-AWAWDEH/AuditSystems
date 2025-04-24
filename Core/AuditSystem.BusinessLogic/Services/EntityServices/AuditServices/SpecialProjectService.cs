using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class SpecialProjectService(
    IRepository<Guid, SpecialProject> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SpecialProjectService> logger) :
    UnifiedCacheService<Guid, SpecialProject, SpecialProjectModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SpecialProject,
        SpecialProjectTags,
        ListTags
    ),
    ISpecialProjectService
{
    private static readonly string[] SpecialProjectTags = ["special-projects", "special-project-list"];
    private static readonly string[] ListTags = ["special-project-list"];
    
    public async Task<Guid> CreateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        var entity = mapper.Map<SpecialProject>(specialProjectModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteSpecialProjectAsync(Guid specialProjectId)
    {
        var cacheKey = GetCacheKey(specialProjectId);

        var entity = await repository.GetByIdAsync(specialProjectId)
            ?? throw new KeyNotFoundException($"SpecialProject with ID {specialProjectId} not found.");    
        
        await repository.DeleteAsync(specialProjectId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<SpecialProjectModel> GetSpecialProjectByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<SpecialProject>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<SpecialProjectModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"SpecialProject with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SpecialProjectModel>(entity);
    }

    public Task<PaginationResult<SpecialProjectModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));

        var entity = mapper.Map<SpecialProject>(specialProjectModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}