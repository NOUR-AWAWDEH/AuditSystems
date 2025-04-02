using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.RiskControls;

public sealed class RiskProgramModel : BaseModel<Guid>
{
    public required Guid RiskControlId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}