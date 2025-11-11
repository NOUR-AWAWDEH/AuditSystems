using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

public interface IObservationValidation
{
     Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId);
     Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId);
}
