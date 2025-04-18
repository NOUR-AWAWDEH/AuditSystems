using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class SpecialProjectModel : BaseModel<Guid>
{
    public required Guid AuditUniverseId { get; set; }
    public required string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required DateTime StartDate { get; set; } = DateTime.UtcNow;
    public required DateTime? EndDate { get; set; }
}