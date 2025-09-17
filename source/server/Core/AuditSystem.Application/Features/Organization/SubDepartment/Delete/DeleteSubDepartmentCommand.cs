using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Delete;

public sealed record DeleteSubDepartmentCommand(
    Guid Id
) : UserAwareCommand<Result>;