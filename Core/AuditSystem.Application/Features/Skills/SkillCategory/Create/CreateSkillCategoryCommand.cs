using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Create;

public sealed record class CreateSkillCategoryCommand : ICommand<Result<CreateSkillCategoryCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
