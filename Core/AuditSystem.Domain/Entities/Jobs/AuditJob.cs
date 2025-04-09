using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs;

public class AuditJob : Entity<Guid>
{
    public required string JobName { get; set; } = string.Empty;
    public required string JobType { get; set; } = string.Empty;
    public required Guid AuditUniverseID { get; set; }

    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
}