using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Skills;

public class Skill : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid CategoryId { get; set; }
    public required Guid SkillSetId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual SkillSet SkillSet { get; set; } = null!;
    public virtual SkillCategory Category { get; set; } = null!;
}