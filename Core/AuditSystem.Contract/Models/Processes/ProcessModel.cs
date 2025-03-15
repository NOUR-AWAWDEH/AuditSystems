using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Processes;

class ProcessModel : BaseModel<Guid>
{
    public required string ProcessName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public required Guid AuditSettingsId { get; set; }
}
