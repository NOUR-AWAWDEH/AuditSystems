using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Jobs;

public sealed class JobSchedulingModel : BaseModel<Guid>
{
    public string AuditableUnit { get; set; } = string.Empty;
    public DateOnly AuditYear { get; set; }
    public DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
