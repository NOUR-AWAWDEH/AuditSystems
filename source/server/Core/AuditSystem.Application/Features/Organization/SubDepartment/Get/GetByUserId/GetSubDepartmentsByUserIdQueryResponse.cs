using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetByUserId;

public sealed record GetSubDepartmentsByUserIdQueryResponse(IReadOnlyList<SubDepartmentModel> SubDepartments);   