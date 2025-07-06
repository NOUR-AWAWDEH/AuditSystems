using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Jobs;

public sealed class JobPrioritizationModel : BaseModel<Guid>
{
    public required string AuditableUnit { get; set; } = string.Empty;
    public required bool SelectForAudit { get; set; }
    public string Comments { get; set; } = string.Empty;
    public required DateOnly SelectedYear { get; set; }
    public required Guid BusinessObjectiveId { get; set; }
    public required Guid RatingId { get; set; }
}