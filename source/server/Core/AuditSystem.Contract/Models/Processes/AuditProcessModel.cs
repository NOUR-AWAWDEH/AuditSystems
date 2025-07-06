using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Processes;

public sealed class AuditProcessModel : BaseModel<Guid>
{
    public required string ProcessName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
