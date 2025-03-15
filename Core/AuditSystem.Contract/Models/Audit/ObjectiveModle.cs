using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class ObjectiveModle : BaseModel<Guid>
{
    public required Guid RiskControlMatrixId { get; set; }
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
