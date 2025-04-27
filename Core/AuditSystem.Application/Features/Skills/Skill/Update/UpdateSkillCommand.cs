using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.Skill.Update;

public sealed record class UpdateSkillCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
    public required Guid CategoryId { get; set; }
    public required Guid SkillSetId { get; set; }
    public string Description { get; set; } = string.Empty;
}
