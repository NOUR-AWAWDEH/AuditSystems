using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class Remark : Entity<Guid>
{
    public required Guid CheckListManagementId { get; set; }
    public string RemarkCommants { get; set; } = string.Empty;

    public virtual ChecklistManagement ChecklistManagement { get; set; } = null!;
}