using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.RiskControls;

public class Objective : Entity<Guid>
{
    public required Guid RiskControlMatrixId { get; set; }
    public required string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual RiskControlMatrix RiskControlMatrix { get; set; } = null!;
    public virtual ICollection<Risk> Risks { get; set; } = null!;
}