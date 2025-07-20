using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Domain.Entities.Processes;

public class AuditProcess : Entity<Guid>
{
    public required string ProcessName { get; set; } = string.Empty;
    public required Guid SubDepartmentId { get; set; }

    // Navigation property
    public virtual SubDepartment SubDepartment { get; set; } = null!;
    public virtual ICollection<SubProcess> SubProcesses { get; set; } = new HashSet<SubProcess>();
}