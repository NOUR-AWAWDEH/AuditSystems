using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Processes;

public sealed class SubProcessModel : BaseModel<Guid>
{
    public required string Particular { get; set; }
    public required Guid AuditProcessId { get; set; }
}