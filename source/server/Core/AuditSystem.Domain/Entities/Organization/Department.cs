using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class Department : Entity<Guid>
{
    public required string Name { get; set; }
    public required Guid SubLocationId { get; set; }

    // Navigation properties
    public virtual SubLocation SubLocation { get; set; } = null!;
    public virtual ICollection<SubDepartment> SubDepartments { get; set; } = new HashSet<SubDepartment>();
}