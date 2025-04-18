using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditDomainService
{
    public Task<Guid> CreateAuditDomainAsync(AuditDomainModel auditDomainModel);
    public Task UpdateAuditDomainAsync(AuditDomainModel auditDomianModel);
    public Task DeleteAuditDomainAsync(Guid auditDomainId);
    public Task<AuditDomainModel> GetAuditDomainByIdAsync(Guid id);
}
