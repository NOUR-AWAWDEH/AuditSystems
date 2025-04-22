using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditUniverseService
{
    public Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel);
    public Task UpdateAuditUniverseAsync(AuditUniverseModel auditUniverseModel);
    public Task DeleteAuditUniverseAsync(Guid auditUniverseId);
    public Task<AuditUniverseModel> GetAuditUniverseByIdAsync(Guid id);
    public Task<PaginationResult<AuditUniverseModel>> Search(string? Name, Page page);
}