using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class Checklist : Entity<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
    public required string Observation { get; set; } = string.Empty;
    public required Guid ChecklistCollectionId { get; set; }

    public virtual ChecklistCollection ChecklistCollection { get; set; } = null!;
}