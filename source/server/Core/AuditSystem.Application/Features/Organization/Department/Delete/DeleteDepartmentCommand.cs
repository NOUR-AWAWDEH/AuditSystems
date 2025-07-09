using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Delete;

public sealed record DeleteDepartmentCommand(
    Guid Id
) : UserAwareCommand<Result>;