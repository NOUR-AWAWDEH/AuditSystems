using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

public sealed class DriftReportData
{
    public CompanyModel? Company { get; init; }
    public LocationModel? Location { get; init; }
    public DepartmentModel? Department { get; init; }
    public SubDepartmentModel? SubDepartment { get; init; }
    public ObjectiveModel? Objective { get; init; }

    public AuditProgramModel? AuditProgram { get; init; }

    public PaginationResult<RiskModel> Risks { get; init; }
        = PaginationResult<RiskModel>.Empty;

    public PaginationResult<ObservationModel> Observations { get; init; }
        = PaginationResult<ObservationModel>.Empty;

    public List<RecommendationModel> Recommendations { get; init; } = [];     
}
