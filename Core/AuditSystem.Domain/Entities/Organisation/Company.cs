using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation;

public class Company : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}