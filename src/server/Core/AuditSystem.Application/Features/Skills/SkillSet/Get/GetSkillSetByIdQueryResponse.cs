using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Application.Features.Skills.SkillSet.Get;

public sealed record class GetSkillSetByIdQueryResponse(SkillSetModel SkillSetModel);