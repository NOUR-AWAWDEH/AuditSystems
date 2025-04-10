using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Tasks.Create;

public sealed record class CreateTaskManagementCommand : ICommand<Result<CreateTaskManagementCommandResponse>>
{
    public required string Requirement { get; set; } = string.Empty;
    public required DateOnly DueDate { get; set; }
    public required string JobName { get; set; } = string.Empty;
    public required string Assignee { get; set; } = string.Empty;
    public required Guid AssignedById { get; set; }
}