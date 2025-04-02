using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class SpecificRiskFactor : Entity<Guid>
{
    public Guid RiskAssessmentId { get; set; }
    public Guid RiskFactorId { get; set; }

    public virtual RiskAssessment RiskAssessments { get; set; } = null!;
    public virtual RiskFactor RiskFactors { get; set; } = null!;
}