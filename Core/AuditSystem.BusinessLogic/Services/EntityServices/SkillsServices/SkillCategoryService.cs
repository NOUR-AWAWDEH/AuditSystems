﻿using AuditSystem.Application.Constants;
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

    public async Task DeleteSkillCategoryAsync(Guid skillCategoryId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(skillCategoryId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Skill Category with ID {skillCategoryId} not found.");
            }

            await repository.DeleteAsync(skillCategoryId);

            var cacheKey = string.Format(CacheKeys.SkillCategory, skillCategoryId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Skill Category.", ex);
        }
    }

    public async Task<SkillCategoryModel> GetSkillCategoryByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.SkillCategory, id);

            var cachedEntity = await cacheService.GetAsync<SkillCategory>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SkillCategoryModel>(cachedEntity); 
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Skill Category with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SkillCategoryTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<SkillCategoryModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get Skill Category by ID {id}.", ex);
        }
    }

    public async Task UpdateSkillCategoryAsync(SkillCategoryModel skillCategoryModel)
    {
        ArgumentNullException.ThrowIfNull(skillCategoryModel, nameof(skillCategoryModel));

        try
        {
            var entity = mapper.Map<SkillCategory>(skillCategoryModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SkillCategory, entity.Id);


            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SkillCategoryTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Skill Category.", ex);
        }
    }
}
