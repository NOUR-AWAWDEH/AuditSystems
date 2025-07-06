using AuditSystem.Domain.Entities.Common;
namespace AuditSystem.Domain.Entities.Audit;

public class AuditUniverseObjective : Entity<Guid>
{
    public required Guid AuditUniverseID { get; set; }
    public required string Impact { get; set; } = string.Empty;
    public int Amount { get; set; }
    public int ImpactAmount { get; set; }
    public double Percentage { get; set; }

    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
}