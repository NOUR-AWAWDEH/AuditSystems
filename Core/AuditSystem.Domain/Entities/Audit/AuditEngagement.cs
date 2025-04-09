using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organisation;

namespace AuditSystem.Domain.Entities.Audit;

public class AuditEngagement : Entity<Guid>
{
    public required string JobName { get; set; } = string.Empty;
    public required DateOnly PlannedStartDate { get; set; }
    public required DateOnly PlannedEndDate { get; set; }
    public required string JobType { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;

    public virtual Location Location { get; set; } = null!;
}