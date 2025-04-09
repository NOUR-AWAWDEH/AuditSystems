using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class RiskAssessment : Entity<Guid>
{
    public required string BusinessObjective { get; set; } = string.Empty;
    public required string NatureThrough { get; set; } = string.Empty;
    public required string PerformRiskAssessment { get; set; } = string.Empty;
}