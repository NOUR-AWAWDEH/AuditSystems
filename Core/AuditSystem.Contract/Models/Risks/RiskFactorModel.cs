using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public class RiskFactorModel : BaseModel<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Factor { get; set; } = string.Empty;

    public object CreateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        throw new NotImplementedException();
    }
}