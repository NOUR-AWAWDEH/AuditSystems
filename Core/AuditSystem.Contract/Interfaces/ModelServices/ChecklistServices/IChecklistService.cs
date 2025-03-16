using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;

public interface IChecklistService
{
    public Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel);
}