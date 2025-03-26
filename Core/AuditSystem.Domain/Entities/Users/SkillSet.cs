using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class SkillSet : Entity<Guid>
{
    public required Guid UserManagementId { get; set; }
    public required Guid SkillId { get; set; }
    public string ProficiencyLevel { get; set; } = string.Empty;

    public virtual UserManagement UserManagement { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
}