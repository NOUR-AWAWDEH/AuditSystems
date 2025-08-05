using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskControlMatrix : Entity<Guid>
{
    public required Guid SubProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual SubProcess SubProcess { get; set; } = null!;
    public virtual ICollection<Objective> Objectives { get; set; } = null!;
}