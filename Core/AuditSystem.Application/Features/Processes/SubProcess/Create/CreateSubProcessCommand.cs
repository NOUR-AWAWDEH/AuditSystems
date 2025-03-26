using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Create;

public sealed record class CreateSubProcessCommand : ICommand<Result<CreateSubProcessCommandResponse>>
{
    public required Guid ProcessId { get; set; }
    public string Particular { get; set; } = string.Empty;
}
