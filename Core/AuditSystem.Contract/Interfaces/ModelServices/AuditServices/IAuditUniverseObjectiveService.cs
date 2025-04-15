using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditUniverseObjectiveService
{
    public Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel);
    public Task UpdateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel);
    public Task DeleteAuditUniverseObjectiveAsync(Guid auditUniverseObjectiveId);
}
