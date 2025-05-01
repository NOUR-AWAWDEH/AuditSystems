using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.RiskControls;

public sealed class RiskControlsModel : BaseModel<Guid>
{
    public required Guid RiskId { get; set; }
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}