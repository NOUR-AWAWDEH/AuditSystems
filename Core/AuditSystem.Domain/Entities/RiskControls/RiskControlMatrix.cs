using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskControlMatrix : Entity<Guid>
{
    public Guid SubProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    
    public virtual SubProcess SubProcess { get; set; } = null!;         
}