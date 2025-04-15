using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillSetService(
    IRepository<Guid, SkillSet> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISkillSetService
{
    private static readonly string[] SkillSetTags = ["skill-sets", "skill-set-list"];
    private static readonly string[] ListTags = ["skill-set-list"]; // Tags for collections only

    public async Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));

        try
        {
            var entity = mapper.Map<SkillSet>(skillSetModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SkillSet, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SkillSetTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SkillSet.", ex);
        }
    }

    public async Task DeleteSkillSetAsync(Guid skillSetId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(skillSetId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"SkillSet with ID {skillSetId} not found.");
            }

            await repository.DeleteAsync(skillSetId);

            var cacheKey = string.Format(CacheKeys.SkillSet, skillSetId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete SkillSet.", ex);
        }
    }

    public async Task UpdateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));

        try
        {
            var entity = mapper.Map<SkillSet>(skillSetModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SkillSet, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SkillSetTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SkillSet.", ex);
        }
    }
}