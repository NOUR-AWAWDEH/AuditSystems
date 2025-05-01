using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditEngagementModel : BaseModel<Guid>
{
    public required string JobName { get; set; } = string.Empty;
    public required DateOnly PlannedStartDate { get; set; }
    public required DateOnly PlannedEndDate { get; set; }
    public required string JobType { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
}
