using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Domain.Entities.Common;

public class Rate : Entity<Guid>
{
    public required RateLevel Level { get; set; }
    public virtual ICollection<Observation> Observations { get; set; } = new List<Observation>();
}