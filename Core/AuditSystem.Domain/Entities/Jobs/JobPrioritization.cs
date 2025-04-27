using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs;

public class JobPrioritization : Entity<Guid>
{
    public required string AuditableUnit { get; set; } = string.Empty;
    public required bool SelectForAudit { get; set; }
    public string Comments { get; set; } = string.Empty;
    public required DateOnly SelectedYear { get; set; }
    public required Guid BusinessObjectiveId { get; set; }
    public required Guid RatingId { get; set; }

    public virtual Rating Rating { get; set; } = null!;
    public virtual BusinessObjective BusinessObjective { get; set; } = null!;
}