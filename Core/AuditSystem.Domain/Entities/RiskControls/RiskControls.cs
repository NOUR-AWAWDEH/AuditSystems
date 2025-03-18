using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskControls : Entity<Guid>
{
    public Guid RiskId { get; set; }
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public Rating Rating { get; set; } = null!;
    public virtual Risk Risk { get; set; } = null!;
}