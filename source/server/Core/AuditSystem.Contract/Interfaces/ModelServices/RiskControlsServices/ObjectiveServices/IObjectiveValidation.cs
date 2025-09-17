using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;

public interface IObjectiveValidation
{
    Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId);
    Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId);
}