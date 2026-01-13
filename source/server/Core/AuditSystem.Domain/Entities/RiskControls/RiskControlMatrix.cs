using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Domain.Entities.RiskControls;

public class RiskControlMatrix : AuditableEntity<Guid>
{
    public required Guid AuditProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual AuditProcess AuditProcess { get; set; } = null!;
    public virtual ICollection<Objective> Objectives { get; set; } = null!;
}