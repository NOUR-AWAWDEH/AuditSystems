using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Jobs;

public class JobScheduling : Entity<Guid>
{
    public int SerialNumber { get; set; }
    public string AuditableUnit { get; set; } = string.Empty;
    public DateOnly AuditYear { get; set; }
    public DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}