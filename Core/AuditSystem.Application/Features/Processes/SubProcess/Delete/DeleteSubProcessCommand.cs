using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;

public sealed class DeleteSubProcessCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}