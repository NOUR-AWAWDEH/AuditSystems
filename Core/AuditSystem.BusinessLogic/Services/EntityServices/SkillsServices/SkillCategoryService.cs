using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillCategoryService(
    IRepository<Guid, SkillCategory> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISkillCategoryService
{
    private static readonly string[] SkillCategoryTags = ["skill-categorys", "skill-Category-list"];
    private static readonly string[] ListTags = ["skill-category-list"]; // Tags for collections only

    public async Task<Guid> CreateSkillCategoryAsync(SkillCategoryModel skillCategoryModel)
    {
        ArgumentNullException.ThrowIfNull(skillCategoryModel, nameof(skillCategoryModel));

        try
        {
            var entity = mapper.Map<SkillCategory>(skillCategoryModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SkillCategory, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SkillCategoryTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Skill Category.", ex);
        }
    }
}
