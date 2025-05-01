using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.RiskControls;

public sealed class RiskControlMatrixModel : BaseModel<Guid>
{
    public required Guid SubProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
}