using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

public interface IAuditJobService
{
    public Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel);
    public Task UpdateAuditJobAsync(AuditJobModel auditJobModel);
    public Task DeleteAuditJobAsync(Guid auditJobId);
    public Task<AuditJobModel> GetAuditJobByIdAsync(Guid Id);
}