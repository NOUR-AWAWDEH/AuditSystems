using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillService(
    IRepository<Guid, Skill> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISkillService
{
    private static readonly string[] SkillTags = ["skills", "skill-list"];
    private static readonly string[] ListTags = ["skill-list"]; // Tags for collections only

    public async Task<Guid> CreateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));

        try
        {
            var entity = mapper.Map<Skill>(skillModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Skill, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SkillTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Skill.", ex);
        }
    }

    public async Task DeleteSkillAsync(Guid skillId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(skillId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Skill with ID {skillId} not found.");
            }
            await repository.DeleteAsync(skillId);

            var cacheKey = string.Format(CacheKeys.Skill, skillId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Skill.", ex);
        }
    }

    public Task<SkillModel> GetSkillByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));

        try
        {
            var entity = mapper.Map<Skill>(skillModel);
            
            await repository.UpdateAsync(entity);
            var cacheKey = string.Format(CacheKeys.Skill, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SkillTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Skill.", ex);
        }
    }
}
