using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

public record OrganizationalData(
    CompanyModel Company,
    LocationModel Location,
    DepartmentModel Department,
    SubDepartmentModel SubDepartment
);