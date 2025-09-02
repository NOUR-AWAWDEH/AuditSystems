using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetByUserId;

public sealed record GetDepartmentsByUserIdQueryResponse(IReadOnlyList<DepartmentModel> Departments);
