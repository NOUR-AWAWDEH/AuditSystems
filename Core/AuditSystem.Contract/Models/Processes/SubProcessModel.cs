using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Processes;

public sealed class SubProcessModel : BaseModel<Guid>
{
    public required Guid ProcessId { get; set; }
    public required string Particular { get; set; } = string.Empty;
}
