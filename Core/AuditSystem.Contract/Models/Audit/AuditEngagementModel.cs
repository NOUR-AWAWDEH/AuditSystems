using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditEngagementModel : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string JobName { get; set; } = string.Empty;
    public DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string JobType { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
}
