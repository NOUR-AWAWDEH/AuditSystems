using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Domain.Entities.Recommendations;
public class Recommendation : AuditableEntity<Guid>
{
    public required string Title { get; set; }
    public required string Description { get; set; } = string.Empty;
    public required Guid ObservationId { get; set; }
    public virtual Observation Observation { get; set; } = null!;
}