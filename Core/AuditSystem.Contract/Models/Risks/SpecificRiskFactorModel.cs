using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public class SpecificRiskFactorModel : BaseModel<Guid>
{
    public required Guid RiskAssessmentId { get; set; }
    public string RiskFactor { get; set; } = string.Empty;
}