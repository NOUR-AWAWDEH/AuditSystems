
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks
{
    public class SpecificRiskFactors : Entity<Guid>
    {
        public Guid RiskAssessmentID { get; set; }
        public string RiskFactor { get; set; } = string.Empty;

        public RiskAssessments RiskAssessments { get; set; } = null!;
    }
}