using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Tasks;

public class TaskManagement : Entity<Guid>
{
    public required string Requirement { get; set; } = string.Empty;
    public required DateOnly DueDate { get; set; }
    public required string JobName { get; set; } = string.Empty;
    public required string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set; }

    public virtual User AssignedBy { get; set; } = null!;
}