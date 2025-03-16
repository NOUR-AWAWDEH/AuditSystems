using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditEngagementService
{
    public Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel);
}
