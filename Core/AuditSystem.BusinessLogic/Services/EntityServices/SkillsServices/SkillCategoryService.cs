using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
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
    BaseEntityCacheService<SkillCategory>(
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

        try
        {
            var entity = mapper.Map<SkillCategory>(skillCategoryModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SkillCategoryModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SkillCategory");
            throw new InvalidOperationException("Failed to create SkillCategory.", ex);
        }
    }

    public async Task DeleteSkillCategoryAsync(Guid skillCategoryId)
    {
        var cacheKey = GetCacheKey(skillCategoryId);
        
        try
        {
            var entity = await repository.GetByIdAsync(skillCategoryId)
                ?? throw new KeyNotFoundException($"Skill Category with ID {skillCategoryId} not found.");

            await repository.DeleteAsync(skillCategoryId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Skill Category with ID {SkillCategoryId} not found", skillCategoryId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Skill Category ID {SkillCategoryId}", skillCategoryId);
            throw new InvalidOperationException($"Failed to delete Skill Category: {skillCategoryId}", ex);
        }
    }

    public async Task<SkillCategoryModel> GetSkillCategoryByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<SkillCategory>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<SkillCategoryModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Skill Category with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SkillCategoryModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Skill Category with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Skill Category with ID '{id}'", ex);
        }
    }

    public async Task UpdateSkillCategoryAsync(SkillCategoryModel skillCategoryModel)
    {
        ArgumentNullException.ThrowIfNull(skillCategoryModel, nameof(skillCategoryModel));

        try
        {
            var entity = mapper.Map<SkillCategory>(skillCategoryModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Skill Category with ID {Id}", skillCategoryModel.Id);
            throw new InvalidOperationException($"Failed to update Skill Category with ID '{skillCategoryModel.Id}'", ex);
        }
    }
}
