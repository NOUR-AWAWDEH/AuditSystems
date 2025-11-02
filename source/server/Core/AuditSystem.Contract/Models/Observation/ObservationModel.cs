
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Observation;

public class ObservationModel : BaseModel<Guid>
{
    public required string Description { get; set; }
    public required string ActionOwner { get; set; }
    public required string Background { get; set; }
    public required string ActionPlan { get; set; }
    public required DateTime ActionDate { get; set; }
    public string? Note { get; set; } = string.Empty;
    public required Guid AuditProgramId { get; set; }
}
