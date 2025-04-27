using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IAuditJobService
{
    public Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel);
    public Task UpdateAuditJobAsync(AuditJobModel auditJobModel);
    public Task DeleteAuditJobAsync(Guid auditJobId);
    public Task<AuditJobModel> GetAuditJobByIdAsync(Guid id);
    public Task<PaginationResult<AuditJobModel>> SearchAsync(string? name, Page page);
}