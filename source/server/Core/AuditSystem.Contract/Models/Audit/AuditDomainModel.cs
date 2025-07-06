using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditDomainModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
}
