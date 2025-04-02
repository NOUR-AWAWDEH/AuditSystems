using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Skill.Create;

public sealed record class CreateSkillCommand : ICommand<Result<CreateSkillCommandResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
