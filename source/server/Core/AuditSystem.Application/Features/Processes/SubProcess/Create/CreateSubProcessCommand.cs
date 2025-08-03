using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Create;

public sealed record class CreateSubProcessCommand(
    string Particular,
    Guid AuditProcessId
) : UserAwareCommand<Result<CreateSubProcessCommandResponse>>;
