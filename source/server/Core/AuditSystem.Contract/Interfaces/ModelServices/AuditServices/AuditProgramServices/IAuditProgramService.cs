using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;

public interface IAuditProgramService
{
    Task<Guid> CreateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId);
    Task UpdateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId);
    Task DeleteAuditProgramAsync(Guid auditProgramId, Guid userId); 
    Task<AuditProgramModel> GetAuditProgramByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<AuditProgramModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid exitControlId, Page? page);
    Task<IReadOnlyList<AuditProgramModel>> GetAuditProgramsByUserIdAsync(Guid userId);
    
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<AuditProgramModel>> SearchAsync(string? name, Page page);
}
