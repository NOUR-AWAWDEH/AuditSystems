using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillSet.Create;

public sealed record class CreateSkillSetCommand : ICommand<Result<CreateSkillSetCommandResponse>>
{
    public required string ProficiencyLevel { get; set; } = string.Empty;
}
