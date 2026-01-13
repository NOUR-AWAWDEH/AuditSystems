using System.Runtime.InteropServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

public class DriftReportModel 
{
    public Guid UserId { get; set; } = Guid.Empty;
    public CompanyModel? Company { get; set; }
    public LocationModel? Location { get; set; }
    public DepartmentModel? Department { get; set; }
    public SubDepartmentModel? SubDepartment { get; set; }
    public ObjectiveModel? Objective { get; set; }
    public AuditProgramModel? AuditProgram { get; set; }
    public DateTime ActionDate { get; set; }
    public string? ActionPlan { get; set; } = string.Empty;
    public bool IsAgreed { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public IReadOnlyList<RiskModel>? Risks { get; init; } = [];
    public IReadOnlyList<ObservationModel>? Observations { get; init; } = [];
    public IReadOnlyList<RecommendationModel>? Recommendations { get; init; } = [];
}