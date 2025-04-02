using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.SkillSet.Create;

public sealed record class CreateSkillSetCommand : ICommand<Result<CreateSkillSetCommandResponse>>
{
    public required Guid UserManagementId { get; set; }
    public required Guid SkillId { get; set; }
    public string ProficiencyLevel { get; set; } = string.Empty;
}
