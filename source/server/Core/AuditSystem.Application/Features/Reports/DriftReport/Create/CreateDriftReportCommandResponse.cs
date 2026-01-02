using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Reports.DriftReport.Create;

public record class CreateDriftReportCommandResponse(
    Guid UserId,
    CompanyModel? Company,
    LocationModel? Location,
    DepartmentModel? Department,
    SubDepartmentModel? SubDepartment,
    ObjectiveModel? Objective,
    AuditProgramModel? AuditProgram,
    DateTime ActionDate,
    string? ActionPlan,
    bool IsAgreed,
    bool IsActive,
    IReadOnlyList<RiskModel>? Risks, 
    IReadOnlyList<ObservationModel>? Observations,
    IReadOnlyList<RecommendationModel>? Recommendations
);