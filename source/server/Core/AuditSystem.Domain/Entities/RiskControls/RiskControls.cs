using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskControls : Entity<Guid>
{
    public required Guid RiskId { get; set; }
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual Risk Risk { get; set; } = null!;
}