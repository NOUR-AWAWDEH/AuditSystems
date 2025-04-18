using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class SubLocation : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;
}