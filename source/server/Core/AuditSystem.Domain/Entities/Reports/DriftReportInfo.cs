namespace AuditSystem.Domain.Entities.Reports;

using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Recommendations;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;

public sealed class DriftReportInfo : AuditableEntity<Guid>
{  
    // Add foreign key IDs for EF Core
    public Guid CompanyId { get; init; }
    public Guid LocationId { get; init; }
    public Guid DepartmentId { get; init; }
    public Guid SubDepartmentId { get; init; }
    public Guid ObjectiveId { get; init; }
    public Guid AuditProgramId { get; init; }
    
    // Navigation properties
    public Company? Company { get; init; }
    public Location? Location { get; init; }
    public Department? Department { get; init; }
    public SubDepartment? SubDepartment { get; init; }
    public Objective? Objective { get; init; }
    public AuditProgram? AuditProgram { get; init; }
    
    public IReadOnlyList<Risk>? Risks { get; init; } = [];
    public IReadOnlyList<Observation>? Observations { get; init; } = [];
    public IReadOnlyList<Recommendation>? Recommendations { get; init; } = [];
}