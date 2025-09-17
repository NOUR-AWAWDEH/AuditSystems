
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Observation;

public class ObservationModel : BaseModel<Guid>
{
    public required string Description { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public required string ActionPlan { get; set; }
    public required DateTime ActionDate { get; set; }
    public required Guid AuditProgramId { get; set; }
}
