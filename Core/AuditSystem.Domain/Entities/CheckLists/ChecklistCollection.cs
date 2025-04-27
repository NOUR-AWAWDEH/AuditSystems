using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Checklists;

public class ChecklistCollection : Entity<Guid>
{
    public virtual ICollection<Checklist> Checklists { get; set; } = null!;
}