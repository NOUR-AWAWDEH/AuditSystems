using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetByuserId;

public sealed record GetDepartmentsByUserIdQuery : UserAwareQuery<Result<GetDepartmentsByUserIdQueryResponse>>;
