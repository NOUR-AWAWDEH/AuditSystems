using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public class RiskFactorModel : BaseModel<Guid>
{
    public required string Factor { get; set; } = string.Empty;
}