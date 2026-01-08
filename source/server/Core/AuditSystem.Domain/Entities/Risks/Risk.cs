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

    // Navigation properties
    public virtual Observation Observation { get; set; } = null!;
}