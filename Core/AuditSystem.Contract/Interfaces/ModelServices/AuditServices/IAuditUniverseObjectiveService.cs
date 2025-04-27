using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditUniverseObjectiveService
{
    public Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel);
    public Task UpdateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel);
    public Task DeleteAuditUniverseObjectiveAsync(Guid auditUniverseObjectiveId);
    public Task<AuditUniverseObjectiveModel> GetAuditUniverseObjectiveByIdAsync(Guid id);
    public Task<PaginationResult<AuditUniverseObjectiveModel>> SearchAsync(string? name, Page page);
}
