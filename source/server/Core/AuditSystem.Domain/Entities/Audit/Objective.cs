using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.Audit;

public class Objective : Entity<Guid>
{
    public required Guid RiskControlMatrixId { get; set; }
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual RiskControlMatrix RiskControlMatrix { get; set; } = null!;
    public virtual ICollection<Risk> Risks { get; set; } = null!;
}