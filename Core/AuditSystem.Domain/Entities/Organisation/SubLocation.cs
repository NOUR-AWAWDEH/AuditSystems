using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation;

public class SubLocation : Entity<Guid>
{
    public required Guid LocationId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual Location Location { get; set; } = null!;
}