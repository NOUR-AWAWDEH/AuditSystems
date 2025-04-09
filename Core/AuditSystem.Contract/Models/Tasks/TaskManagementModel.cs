using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Tasks;

public sealed class TaskManagementModel : BaseModel<Guid>
{
    public required string Requirement { get; set; } = string.Empty;
    public required DateOnly DueDate { get; set; }
    public required string JobName { get; set; } = string.Empty;
    public required string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set; }
}