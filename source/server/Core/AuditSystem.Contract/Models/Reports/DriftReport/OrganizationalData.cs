namespace AuditSystem.Contract.Models.Reports.DriftReport;

public record OrganizationalData(
    dynamic Company,
    dynamic Location,
    dynamic Department,
    dynamic SubDepartment
);