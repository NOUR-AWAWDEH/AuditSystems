using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Risks;

public class Risk : Entity<Guid>
{
    public required Guid ObjectiveId { get; set; }
    public required string RiskName { get; set; }
    public required Guid RateId { get; set; }
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual Rate Rate { get; set; } = null!;
    public virtual Objective Objective { get; set; } = null!;
}