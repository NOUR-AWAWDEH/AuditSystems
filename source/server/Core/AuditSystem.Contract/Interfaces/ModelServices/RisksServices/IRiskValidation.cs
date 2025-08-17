using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskValidation
{
    Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId);
    Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId);
}