
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Observation;

public class ObservationModel : BaseModel<Guid>
{
    public required string Highlight { get; set; }
    public required string Background { get; set; }
    public required bool IsActive { get; set; }
    public string? Note { get; set; } = string.Empty;
    public required Guid AuditProgramId { get; set; }
    public required Guid RateId { get; set; }
}
