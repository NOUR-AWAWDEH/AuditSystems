using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;

public interface IAuditProgramValidation
{
    Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId);
    Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId);
}