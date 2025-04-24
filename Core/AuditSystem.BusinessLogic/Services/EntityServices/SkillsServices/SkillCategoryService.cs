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

internal sealed class SkillCategoryService(
    IRepository<Guid, SkillCategory> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SkillCategoryService> logger) : 
    UnifiedCacheService<Guid, SkillCategory, SkillCategoryModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SkillCategory,
        SkillCategoryTags,
        ListTags
    ),
    ISkillCategoryService
{
    private static readonly string[] SkillCategoryTags = ["skill-categorys", "skill-Category-list"];
    private static readonly string[] ListTags = ["skill-category-list"];

    public async Task<Guid> CreateSkillCategoryAsync(SkillCategoryModel skillCategoryModel)
    {
        ArgumentNullException.ThrowIfNull(skillCategoryModel, nameof(skillCategoryModel));
        
        var entity = mapper.Map<SkillCategory>(skillCategoryModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteSkillCategoryAsync(Guid skillCategoryId)
    {
        var cacheKey = GetCacheKey(skillCategoryId);
        
        var entity = await repository.GetByIdAsync(skillCategoryId)
            ?? throw new KeyNotFoundException($"Skill Category with ID {skillCategoryId} not found.");

        await repository.DeleteAsync(skillCategoryId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<SkillCategoryModel> GetSkillCategoryByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var entity = await cacheService.GetAsync<SkillCategory>(cacheKey);
        if (entity != null)
            return mapper.Map<SkillCategoryModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Skill Category with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SkillCategoryModel>(entity);
    }

    public Task<PaginationResult<SkillCategoryModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSkillCategoryAsync(SkillCategoryModel skillCategoryModel)
    {
        ArgumentNullException.ThrowIfNull(skillCategoryModel, nameof(skillCategoryModel));

        var entity = mapper.Map<SkillCategory>(skillCategoryModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
