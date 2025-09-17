using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetByUserId;

public sealed record GetSubDepartmentsByUserIdQuery : UserAwareQuery<Result<GetSubDepartmentsByUserIdQueryResponse>>;
