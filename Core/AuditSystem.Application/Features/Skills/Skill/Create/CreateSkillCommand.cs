using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.Skill.Create;

public sealed record class CreateSkillCommand : ICommand<Result<CreateSkillCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid CategoryId { get; set; }
    public required Guid SkillSetId { get; set; }
    public string Description { get; set; } = string.Empty;
}
