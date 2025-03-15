using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Processes;

public class SupProcessModel : BaseModel<Guid>
{
    public required Guid ProcessId { get; set; }
    public required string Particular { get; set; } = string.Empty;
}
