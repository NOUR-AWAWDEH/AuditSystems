using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class BusinessObjectiveModel : BaseModel<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Impact { get; set; } = string.Empty;
    public int Amount { get; set; }
}
