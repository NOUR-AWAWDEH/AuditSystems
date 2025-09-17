using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class SubLocation : Entity<Guid>
{
    public required string Name { get; set; }
    public string Address { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }

    // Navigation properties
    public virtual Location Location { get; set; } = null!;
    public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();
}