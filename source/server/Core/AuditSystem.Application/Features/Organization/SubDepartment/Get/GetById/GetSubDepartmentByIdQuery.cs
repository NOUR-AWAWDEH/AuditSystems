using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetById;

public sealed record class GetSubDepartmentByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetSubDepartmentByIdQueryResponse>>;