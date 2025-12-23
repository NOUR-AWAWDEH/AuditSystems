using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organization;

public class Company : Entity<Guid>
{
    public required string Name { get; init; }

    // Navigation properties
    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>(); // One-to-Many
}