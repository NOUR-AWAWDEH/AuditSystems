using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class ChecklistService(
    IRepository<Guid, Checklist> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ChecklistService> logger) :
    UnifiedCacheService<Guid, Checklist, ChecklistModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Checklist,
        ChecklistTags,
        ListTags
    ),
    IChecklistService
{
    private static readonly string[] ChecklistTags = ["checklists", "checklist-list"];
    private static readonly string[] ListTags = ["checklist-list"];
    
    public async Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        var entity = mapper.Map<Checklist>(checklistModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteChecklistAsync(Guid checklistId)
    {
        var cacheKey = GetCacheKey(checklistId);

        var entity = await repository.GetByIdAsync(checklistId)
            ?? throw new KeyNotFoundException($"Checklist with ID {checklistId} not found.");

        await repository.DeleteAsync(checklistId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<ChecklistModel> GetChecklistByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<Checklist>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<ChecklistModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Checklist with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<ChecklistModel>(entity);
    }

    public async Task<PaginationResult<ChecklistModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        var (cacheKey, cacheCountKey) = GetSearchCacheKeys(name, page);
        
        var cachedResult = await GetFromCacheAsync<ChecklistModel>(cacheKey, cacheCountKey, page);
        if (cachedResult != null)
            return cachedResult;
        
        return await GetFromRepositoryAsync<ChecklistModel>(cacheKey, cacheCountKey, page, name);
    }

    public async Task UpdateChecklistAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        var entity = mapper.Map<Checklist>(checklistModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}