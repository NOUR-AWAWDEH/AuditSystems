using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public sealed class SubLocationModel : BaseModel<Guid>
{
    public required Guid LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
}
