using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.Skill.Delete;

public sealed class DeleteSkillCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}