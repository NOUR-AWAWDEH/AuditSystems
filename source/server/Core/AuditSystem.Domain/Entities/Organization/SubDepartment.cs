using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organization;

public class SubDepartment : AuditableEntity<Guid>
{
    public required string Name { get; set; }
    public required Guid DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();
    public virtual ICollection<AuditProcess> AuditProcesses { get; set; } = new HashSet<AuditProcess>();
}
