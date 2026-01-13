namespace AuditSystem.Domain.Entities.Reports;
using AuditSystem.Domain.Entities.Common;

public sealed class DriftReport : AuditableEntity<Guid>
{  
    public Guid CompanyId { get; init; }
    public Guid LocationId { get; init; }
    public Guid DepartmentId { get; init; }
    public Guid SubDepartmentId { get; init; }
    public Guid ObjectiveId { get; init; }
    public Guid AuditProgramId { get; init; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public string ActionPlan { get; set; } = string.Empty;
    public bool IsAgreed { get; set; } = false;
    public bool IsActive { get; set; } = true;
}