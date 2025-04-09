using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.Risks;

public class Risk : Entity<Guid>
{
    public required Guid ObjectiveId { get; set; }
    public required string RiskName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual Objective Objective { get; set; } = null!;
}