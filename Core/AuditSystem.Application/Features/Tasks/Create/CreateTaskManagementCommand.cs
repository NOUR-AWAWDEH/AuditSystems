using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Tasks.Create;

public sealed record class CreateTaskManagementCommand : ICommand<Result<CreateTaskManagementCommandResponse>>
{
    public string Requirement { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set; }
}