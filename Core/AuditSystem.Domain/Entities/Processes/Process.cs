using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Processes;

public class Process : Entity<Guid>
{
    public Guid AuditSettingsId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual Rating Rating { get; set; } = null!;
    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
}