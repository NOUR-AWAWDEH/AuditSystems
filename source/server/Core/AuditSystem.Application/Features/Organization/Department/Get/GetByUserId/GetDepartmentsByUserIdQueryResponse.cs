using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetByuserId;

public sealed record GetDepartmentsByUserIdQueryResponse(ICollection<DepartmentModel> Departments);