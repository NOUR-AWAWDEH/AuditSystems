using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;

public interface IChecklistService
{
    public Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel);
    public Task UpdateChecklistAsync(ChecklistModel checklistModel);
    public Task DeleteChecklistAsync(Guid checklistId);
    public Task<ChecklistModel> GetChecklistByIdAsync(Guid id);
    public Task<PaginationResult<ChecklistModel>> SearchAsync(string? name, Page page);
}