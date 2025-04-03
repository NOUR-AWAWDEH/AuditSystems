using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Users;
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
}