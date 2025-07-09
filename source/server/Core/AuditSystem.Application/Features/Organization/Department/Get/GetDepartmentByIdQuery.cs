using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Get;

public sealed record GetDepartmentByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetDepartmentByIdQueryResponse>>;
