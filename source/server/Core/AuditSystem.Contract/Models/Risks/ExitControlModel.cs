using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Models.Risks;

public class  ExitControlModel : BaseModel<Guid> 
{
    public required Guid RiskId { get; set; } = Guid.Empty;
    public required string Description { get; set; } = string.Empty;
    public required bool Inaquate { get; set; }
}