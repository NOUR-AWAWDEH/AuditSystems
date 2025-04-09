using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Audit;

public class Objective : Entity<Guid>
{
    public required Guid RiskControlMatrixId { get; set; }
    public required Guid  RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual RiskControlMatrix RiskControlMatrix { get; set; } = null!;
}