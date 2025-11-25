using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.Common;

public class Rate : Entity<Guid>
{
    public required RateLevel Level { get; set; }
}