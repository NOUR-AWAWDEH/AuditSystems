using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Update;

public sealed record class UpdateSubDepartmentCommand(
    Guid Id,
    string Name,
    Guid DepartmentId
) : UserAwareCommand<Result>;
