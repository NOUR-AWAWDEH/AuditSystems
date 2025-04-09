using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class Remark : Entity<Guid>
{
    public string RemarkCommants { get; set; } = string.Empty;
    public required Guid CheckListCollectionId { get; set; }

    public virtual ChecklistCollection ChecklistCollection { get; set; } = null!;
}