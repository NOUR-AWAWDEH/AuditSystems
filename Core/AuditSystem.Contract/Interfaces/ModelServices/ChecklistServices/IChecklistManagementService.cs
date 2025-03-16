using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;

public interface IChecklistManagementService
{
    public Task<Guid> CreateChecklistAsync(ChecklistManagementModel checklistModel);
}
