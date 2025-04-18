using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Update;

public sealed record class UpdateSkillCategoryCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
