using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Risks;

public class Risk : AuditableEntity<Guid>
{
    public required Guid ObservationId { get; set; }
    public required string Factor { get; set; }
    public string Impact { get; set; } = string.Empty;
    public required string RiskName { get; set; }
    public required string Description { get; set; }
    public required Guid ObjectiveId { get; set; }

    // Navigation properties
    public virtual Objective Objective { get; set; } = null!;
    public virtual Observation Observation { get; set; } = null!;
}