using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Delete;

public sealed class DeleteSkillCategoryCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}