using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation;

public class Department : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public virtual ICollection<SubDepartment> SubDepartments { get; set; } = new HashSet<SubDepartment>();
}