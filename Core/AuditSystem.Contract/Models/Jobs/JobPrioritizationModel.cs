using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Jobs;

public sealed class JobPrioritizationModel : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string AuditableUnit { get; set; } = string.Empty;
    public required Guid BusinessObjectiveId { get; set; }
    public required Guid RatingId { get; set; }
    public bool SelectForAudit { get; set; }
    public string Comments { get; set; } = string.Empty;
    public DateOnly SelectYear { get; set; }
}