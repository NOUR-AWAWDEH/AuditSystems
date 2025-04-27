using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillSetService(
    IRepository<Guid, SkillSet> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SkillSetService> logger) : 
    UnifiedCacheService<Guid, SkillSet, SkillSetModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SkillSet,
        SkillSetTags,
        ListTags
    ),
    ISkillSetService
{
    private static readonly string[] SkillSetTags = ["skill-sets", "skill-set-list"];
    private static readonly string[] ListTags = ["skill-set-list"];

    public async Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));

        var entity = mapper.Map<SkillSet>(skillSetModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteSkillSetAsync(Guid skillSetId)
    {
        var cacheKey = GetCacheKey(skillSetId);
        
        var entity = await repository.GetByIdAsync(skillSetId)
            ?? throw new KeyNotFoundException($"SkillSet with ID {skillSetId} not found.");

        await repository.DeleteAsync(skillSetId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<SkillSetModel> GetSkillSetByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<SkillSet>(cacheKey);
        if (entity != null)
            return mapper.Map<SkillSetModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"SkillSet with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SkillSetModel>(entity);
    }

    public async Task<PaginationResult<SkillSetModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SkillSetModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<SkillSetModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));
        
        var entity = mapper.Map<SkillSet>(skillSetModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}