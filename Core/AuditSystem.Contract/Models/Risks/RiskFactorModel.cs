using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public class RiskFactorModel : BaseModel<Guid>
{
    public Guid AuditorSettingsId { get; set; }
    public string Factor { get; set; } = string.Empty;
}