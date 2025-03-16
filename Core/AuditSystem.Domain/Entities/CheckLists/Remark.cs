using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class Remark : Entity<Guid>
{
    public Guid CheckListManagementId { get; set; }
    public string Remarkcommants { get; set; } = string.Empty;

    public virtual ChecklistManagement ChecklistManagement { get; set; } = null!;
}