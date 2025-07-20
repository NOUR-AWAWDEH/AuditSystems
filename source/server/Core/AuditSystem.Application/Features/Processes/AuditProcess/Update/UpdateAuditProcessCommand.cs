using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Update;

public sealed record class UpdateAuditProcessCommand(
    Guid Id,
    string ProcessName,
    Guid SubDepartmentId
) : UserAwareCommand<Result>;
