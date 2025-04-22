using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Get;

public sealed record class GetDepartmentByIdQueryResponse(DepartmentModel DepartmentModel);
