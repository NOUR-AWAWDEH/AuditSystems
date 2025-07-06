using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class BusinessObjectiveModel : BaseModel<Guid>
{
    public required string Impact { get; set; } = string.Empty;
    public required int Amount { get; set; }
}
