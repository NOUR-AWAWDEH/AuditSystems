using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Skills;

public class SkillSet : Entity<Guid>
{
    public required string ProficiencyLevel { get; set; } = string.Empty;
    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}