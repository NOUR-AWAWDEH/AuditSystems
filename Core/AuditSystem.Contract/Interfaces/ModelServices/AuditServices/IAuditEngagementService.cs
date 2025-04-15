using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditEngagementService
{
    public Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel);
    public Task UpdateAuditEngagementAsync(AuditEngagementModel auditEngagementModel);
    public Task DeleteAuditEngagementAsync(Guid auditEngagementId);
}
