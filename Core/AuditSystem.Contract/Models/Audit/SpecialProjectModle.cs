using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class SpecialProjectModle : BaseModel<Guid>
{
    public required Guid AuditUniverseId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
}