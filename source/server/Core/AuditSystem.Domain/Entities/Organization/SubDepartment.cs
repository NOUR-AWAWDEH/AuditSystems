using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Domain.Entities.Organization;

public class SubDepartment : Entity<Guid>
{
    public required string Name { get; set; }
    public required Guid DepartmentId { get; set; }

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<AuditProcess> AuditProcesses { get; set; } = new HashSet<AuditProcess>();
}