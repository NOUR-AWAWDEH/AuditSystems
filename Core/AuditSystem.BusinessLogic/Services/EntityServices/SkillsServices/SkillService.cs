using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillService(
    IRepository<Guid, Skill> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SkillService> logger)
    : BaseEntityCacheService<Skill>(
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

        try
        {
            var entity = mapper.Map<Skill>(skillModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "AutoMapper mapping exception occurred.");
            throw new ArgumentException("Failed to map SkillModel to Skill.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the Skill.");
            throw new InvalidOperationException("Failed to create Skill.", ex);
        }
    }

    public async Task DeleteSkillAsync(Guid skillId)
    {
        var cacheKey = GetCacheKey(skillId);
        
        try
        {
            var entity = await repository.GetByIdAsync(skillId)
                ?? throw new KeyNotFoundException($"Skill with ID {skillId} not found.");

            await repository.DeleteAsync(skillId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Skill with ID {SkillId} not found", skillId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Skill ID {SkillId}", skillId);
            throw new InvalidOperationException($"Failed to delete Skill: {skillId}", ex);
        }
    }

    public async Task<SkillModel> GetSkillByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<Skill>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<SkillModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Skill with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SkillModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Skill with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Skill with ID '{id}'", ex);
        }
    }

    public async Task UpdateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));
        
        try
        {
            var entity = mapper.Map<Skill>(skillModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Skill with ID {Id}", skillModel.Id);
            throw new InvalidOperationException($"Failed to update Skill with ID '{skillModel.Id}'", ex);
        }
    }
}
