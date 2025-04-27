using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Tasks.Delete;

public sealed class DeleteTaskManagementCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
