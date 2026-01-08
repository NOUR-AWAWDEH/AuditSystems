using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskValidation
{
    Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId);
    Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId);
}