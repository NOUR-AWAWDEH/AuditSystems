using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;

public sealed record DeleteSubProcessCommand : UserAwareCommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}