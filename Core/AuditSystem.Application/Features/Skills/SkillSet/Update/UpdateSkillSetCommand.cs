using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillSet.Update;

public sealed record class UpdateSkillSetCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string ProficiencyLevel { get; set; } = string.Empty;
}
