using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organization;

public sealed class SubLocationModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
}
