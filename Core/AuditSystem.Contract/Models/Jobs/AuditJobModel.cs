using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Jobs;

public sealed class AuditJobModel : BaseModel<Guid>
{
    public required Guid AuditUniverseID { get; set; }
    public required int SerialNumber { get; set; }
    public required string JobName { get; set; } = string.Empty;
    public required string JobType { get; set; } = string.Empty;
}
