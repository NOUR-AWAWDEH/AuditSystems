using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditUniverseObjectiveModel : BaseModel<Guid>
{
    public Guid AuditUniverseID { get; set; }
    public string Impact { get; set; } = string.Empty;
    public int Amount { get; set; }
    public int ImpactAmount { get; set; }
    public double Percentage { get; set; }
}
