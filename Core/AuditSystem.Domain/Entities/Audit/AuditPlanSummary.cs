using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Audit;

public class AuditPlanSummary : Entity<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Component { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ExampleDetails { get; set; } = string.Empty;

    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
}