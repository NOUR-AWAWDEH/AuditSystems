using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Domain.Entities.Processes;

public class AuditProcess : AuditableEntity<Guid>
{
    public required string ProcessName { get; set; }
    public required Guid SubDepartmentId { get; set; }

    // Navigation property
    public virtual SubDepartment SubDepartment { get; set; } = null!;
    public virtual ICollection<AuditSystem.Domain.Entities.RiskControls.Objective> Objectives { get; set; } = [];
    public virtual ICollection<SubProcess> SubProcesses { get; set; } = [];
}