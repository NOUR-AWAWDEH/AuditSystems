using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.RiskControls;

public sealed class ObjectiveModel : BaseModel<Guid>
{
    public required Guid RiskControlMatrixId { get; set; }
    public required string Description { get; set; }
}