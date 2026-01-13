using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

public record ControlData(
    ObjectiveModel Objective,
    IReadOnlyList<RiskModel> Risks,
    AuditProgramModel AuditProgram,
    IReadOnlyList<ObservationModel> Observations
);