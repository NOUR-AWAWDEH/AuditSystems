using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Processes;

public class SubProcess : Entity<Guid>
{
    public required Guid ProcessId { get; set; }
    public string Particular { get; set; } = string.Empty;

    public virtual AuditProcess Process { get; set; } = null!;
}