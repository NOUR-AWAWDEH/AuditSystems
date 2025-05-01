using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class Location : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public virtual ICollection<SubLocation> SubLocations { get; set; } = new HashSet<SubLocation>();
}