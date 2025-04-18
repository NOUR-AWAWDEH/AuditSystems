﻿using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

public interface ISkillCategoryService
{
    public Task<Guid> CreateSkillCategoryAsync(SkillCategoryModel skillCategoryModel);
    public Task UpdateSkillCategoryAsync(SkillCategoryModel skillCategoryModel);
    public Task DeleteSkillCategoryAsync(Guid skillCategoryId);
    public Task<SkillCategoryModel> GetSkillCategoryByIdAsync(Guid id);
}
