using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs;

public class JobScheduling : Entity<Guid>
{
    public required string AuditableUnit { get; set; } = string.Empty;
    public required DateOnly AuditYear { get; set; }
    public required DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}