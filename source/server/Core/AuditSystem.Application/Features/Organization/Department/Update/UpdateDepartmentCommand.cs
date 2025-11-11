using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Update;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    Guid LocationId
) : UserAwareCommand<Result>;