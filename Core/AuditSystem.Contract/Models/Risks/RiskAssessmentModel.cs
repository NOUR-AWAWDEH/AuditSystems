using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public sealed class RiskAssessmentModel : BaseModel<Guid>
{
    public required string BusinessObjective { get; set; } = string.Empty;
    public required string NatureThrough { get; set; } = string.Empty;
    public required string PerformRiskAssessment { get; set; } = string.Empty;
}
