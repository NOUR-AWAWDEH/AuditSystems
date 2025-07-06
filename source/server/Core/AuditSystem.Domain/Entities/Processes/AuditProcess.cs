using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Processes;

public class AuditProcess : Entity<Guid>
{
    public required string ProcessName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual ICollection<SubProcess> SubProcesses { get; set; } = new HashSet<SubProcess>();
}