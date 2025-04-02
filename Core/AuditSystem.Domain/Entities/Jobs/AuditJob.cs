using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs;

public class AuditJob : Entity<Guid>
{
    public Guid AuditUniverseID { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;

    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
}