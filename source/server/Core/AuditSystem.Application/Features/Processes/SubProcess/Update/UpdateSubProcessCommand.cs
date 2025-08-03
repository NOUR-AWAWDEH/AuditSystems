using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Update;

public sealed record class UpdateSubProcessCommand : UserAwareCommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Particular { get; set; } = string.Empty;
    public required Guid AuditProcessId { get; set; } = Guid.Empty;
}
