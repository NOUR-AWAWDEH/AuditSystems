using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Skills.SkillSet.Delete;

public sealed class DeleteSkillSetCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}