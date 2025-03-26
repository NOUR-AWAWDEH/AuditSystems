using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskProgram : Entity<Guid>
{
    public Guid RiskControlId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual RiskControls RiskControl { get; set; } = null!;
}
