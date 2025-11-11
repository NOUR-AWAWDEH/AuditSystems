using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;

public interface IRiskControlMatrixValidation
{
    Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId);
    Task<SubProcess> ValidateSubProcessOwnershipAsync(Guid subProcessId, Guid userId);
}