using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

public interface IExitControlValidation
{
    Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId);
    Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId);
}