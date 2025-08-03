using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;

public interface IAuditProcessService
{
    Task<Guid> CreateAuditProcessAsync(AuditProcessModel auditProcessModel, Guid userId);
    Task UpdateAuditProcessAsync(AuditProcessModel auditProcessModel, Guid userId);
    Task DeleteAuditProcessAsync(Guid auditProcessId, Guid userId);
    Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id, Guid userId);
    Task<ICollection<AuditProcessModel>> GetAuditProcessesByUserIdAsync(Guid userId);

    Task<PaginationResult<AuditProcessModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid subDepartmentId, Page? page);
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<AuditProcessModel>> SearchAsync(string? name, Page page);
}
