
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class SpecificRiskFactor : Entity<Guid>
{
    public Guid RiskAssessmentId { get; set; }
    public string RiskFactor { get; set; } = string.Empty;

    public virtual RiskAssessment RiskAssessments { get; set; } = null!;
}