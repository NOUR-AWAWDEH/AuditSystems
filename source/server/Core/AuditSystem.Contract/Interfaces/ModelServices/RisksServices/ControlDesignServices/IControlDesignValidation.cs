using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

public interface IControlDesignValidation
{
    Task<ControlDesign> ValidateControlDesignOwnershipAsync(Guid controlDesignId ,Guid userId);
    Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId);
}
