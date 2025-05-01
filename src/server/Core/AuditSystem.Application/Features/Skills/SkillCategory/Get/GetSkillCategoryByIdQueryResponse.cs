using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Get;

public sealed record class GetSkillCategoryByIdQueryResponse(SkillCategoryModel SkillCategoryModel);