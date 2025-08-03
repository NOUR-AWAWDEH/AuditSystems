using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Processes;

public class SubProcess : Entity<Guid>
{
    public required string Particular { get; set; } = string.Empty;
    public required Guid AuditProcessId { get; set; }

    // Navigation property
    public virtual AuditProcess AuditProcess { get; set; } = null!;
    public virtual ICollection<RiskControlMatrix> RiskControlMatrices { get; set; } = new HashSet<RiskControlMatrix>();
}