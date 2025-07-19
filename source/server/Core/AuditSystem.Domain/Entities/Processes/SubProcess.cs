using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Processes;

public class SubProcess : Entity<Guid>
{
    public required string Particular { get; set; } = string.Empty;
    public required Guid AuditProcessId { get; set; }

    // Navigation property
    public virtual AuditProcess AuditProcess { get; set; } = null!;
}