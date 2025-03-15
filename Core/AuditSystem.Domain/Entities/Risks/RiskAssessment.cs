using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class RiskAssessment : Entity<Guid>
{
    public int SerialNumber { get; set; }
    public string BusinessObjective { get; set; } = string.Empty;
    public string NatureThrough { get; set; } = string.Empty;
    public string PerformRiskAssessment { get; set; } = string.Empty;
}