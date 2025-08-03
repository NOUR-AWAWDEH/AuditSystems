using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;


public interface ISubProcessService
{
    Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel, Guid userId);
    Task UpdateSubProcessAsync(SubProcessModel subProcessModel, Guid userId);
    Task DeleteSubProcessAsync(Guid subProcessId, Guid userId);
    Task<SubProcessModel> GetSubProcessByIdAsync(Guid id, Guid userId);
    Task<ICollection<SubProcessModel>> GetSubProcessesByUserIdAsync(Guid userId);
    
    Task<PaginationResult<SubProcessModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid auditProcessId, Page? page);
    
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<SubProcessModel>> SearchAsync(string? name, Page page);
}