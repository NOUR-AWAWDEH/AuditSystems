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

internal sealed class SkillService(
    IRepository<Guid, Skill> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SkillService> logger) : 
    UnifiedCacheService<Guid, Skill, SkillModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Skill,
        SkillTags,
        ListTags
    ),
    ISkillService
{
    private static readonly string[] SkillTags = ["skills", "skill-list"];
    private static readonly string[] ListTags = ["skill-list"];

    public async Task<Guid> CreateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));
        
        var entity = mapper.Map<Skill>(skillModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }

    public async Task DeleteSkillAsync(Guid skillId)
    {
        var cacheKey = GetCacheKey(skillId);
       
        var entity = await repository.GetByIdAsync(skillId)
            ?? throw new KeyNotFoundException($"Skill with ID {skillId} not found.");

        await repository.DeleteAsync(skillId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<SkillModel> GetSkillByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<Skill>(cacheKey);
        if (entity != null)
            return mapper.Map<SkillModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Skill with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SkillModel>(entity);
    }

    public Task<PaginationResult<SkillModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));

        var entity = mapper.Map<Skill>(skillModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
