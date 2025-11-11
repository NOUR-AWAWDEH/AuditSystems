using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.Common;

public class Rate : Entity<Guid>
{
    public required RateLevel Level { get; set; }

    // Navigation properties
    public virtual ICollection<Risk> Risks { get; set; } = new HashSet<Risk>();
}