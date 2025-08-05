using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class ExitRisk : Entity<Guid>
{
    public required Guid RiskId { get; set; }
    public required string Description { get; set; } = string.Empty;
    public bool NotEnough { get; set; }

    // Navigation properties
    public virtual Risk Risk { get; set; } = null!;
}