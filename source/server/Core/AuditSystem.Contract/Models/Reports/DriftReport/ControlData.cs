using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

public record ControlData(
    ObjectiveModel Objective,
    PaginationResult<RiskModel> Risks,
    AuditProgramModel AuditProgram,
    PaginationResult<ObservationModel> Observations
);
