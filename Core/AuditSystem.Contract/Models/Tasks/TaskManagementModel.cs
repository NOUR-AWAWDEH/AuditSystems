using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Tasks;

public sealed class TaskManagementModel : BaseModel<Guid>
{
    public string Requirement { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set; }
}