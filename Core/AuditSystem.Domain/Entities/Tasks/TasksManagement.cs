using AuditSystem.Domain.Entities.Users;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Tasks;

public class TaskManagement : Entity<Guid>
{
    public string Requirement { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set;}

    public virtual User AssignedBy { get; set; } = null!;
}