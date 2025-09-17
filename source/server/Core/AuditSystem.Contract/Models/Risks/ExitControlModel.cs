using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Models.Risks;

public class ExitControlModel : BaseModel<Guid> 
{
    public required Guid RiskId { get; set; }
    public required string Description { get; set; }
    public required bool Inadequate { get; set; }
}