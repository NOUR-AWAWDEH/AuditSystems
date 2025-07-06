using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;

public interface IRemarkService
{
    public Task<Guid> CreateRemarkAsync(RemarkModel remarkModel);
    public Task UpdateRemarkAsync(RemarkModel remarkModel);
    public Task DeleteRemarkAsync(Guid remarkId);
    public Task<RemarkModel> GetRemarkByIdAsync(Guid id);
    public Task<PaginationResult<RemarkModel>> SearchAsync(string? name, Page page);
}
