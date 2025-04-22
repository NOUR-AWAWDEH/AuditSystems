using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.Skill.Get;

public sealed record class GetSkillByIdQueryResponse(SkillModel SkillModel);