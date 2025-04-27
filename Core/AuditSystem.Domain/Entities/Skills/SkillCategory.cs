using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Skills;

public class SkillCategory : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
