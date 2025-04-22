using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditEngagementService
{
    public Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel);
    public Task UpdateAuditEngagementAsync(AuditEngagementModel auditEngagementModel);
    public Task DeleteAuditEngagementAsync(Guid auditEngagementId);
    public Task<AuditEngagementModel> GetAuditEngagementByIdAsync(Guid id);
    public Task<PaginationResult<AuditEngagementModel>> GetAllAuditEngagementsAsync(string? Name, Page page);
}
