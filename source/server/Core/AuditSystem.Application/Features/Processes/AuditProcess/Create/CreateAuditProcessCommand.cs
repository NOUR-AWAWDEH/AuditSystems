using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed record class CreateAuditProcessCommand(
    string ProcessName,
    Guid SubDepartmentId
) : UserAwareCommand<Result<CreateAuditProcessCommandResponse>>;
