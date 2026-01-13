namespace AuditSystem.Contract.Models.Reports.DriftReport;

public class GenerateDriftReportRequest
{
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid CompanyId { get; set; } = Guid.Empty;
    public Guid LocationId { get; set; } = Guid.Empty;
    public Guid DepartmentId { get; set; } = Guid.Empty;
    public Guid SubDepartmentId { get; set; } = Guid.Empty;
    public Guid ObjectiveId { get; set; }  = Guid.Empty;
    public Guid AuditProgramId { get; set; } = Guid.Empty;

    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public string? ActionPlan { get; set; } = string.Empty;
    public bool IsActive { get; set;} = true;
    public bool IsAgreed { get; set;} = false;
}