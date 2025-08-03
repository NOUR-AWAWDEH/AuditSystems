using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Update;

public sealed record UpdateSubProcessCommand(
    Guid Id,
    string Particular,
    Guid AuditProcessId
) : UserAwareCommand<Result>;