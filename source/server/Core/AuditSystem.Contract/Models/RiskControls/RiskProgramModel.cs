using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.RiskControls;

public sealed class RiskProgramModel : BaseModel<Guid>
{
    public required Guid RiskControlId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}