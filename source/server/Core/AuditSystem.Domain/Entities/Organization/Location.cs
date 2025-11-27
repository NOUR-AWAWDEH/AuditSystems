using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class Location : AuditableEntity<Guid>
{
    public required string Name { get; set; }
    public string? Address { get; set; } = string.Empty;

    public required Guid CompanyId { get; set; }
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();
}
