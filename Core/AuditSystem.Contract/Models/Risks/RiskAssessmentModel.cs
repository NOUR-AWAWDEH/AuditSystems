using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public sealed class RiskAssessmentModel : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string BusinessObjective { get; set; } = string.Empty;
    public string NatureThrough { get; set; } = string.Empty;
    public string PerformRiskAssessment { get; set; } = string.Empty;
}
