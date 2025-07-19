using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organization;

public class Company : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>(); // One-to-Many
}