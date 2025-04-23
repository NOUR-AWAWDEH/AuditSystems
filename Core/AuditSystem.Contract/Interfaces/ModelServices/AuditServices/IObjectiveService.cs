using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IObjectiveService
{
    public Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel);
    public Task UpdateObjectiveAsync(ObjectiveModel objectiveModel);
    public Task DeleteObjectiveAsync(Guid objectiveId);
    public Task<ObjectiveModel> GetObjectiveByIdAsync(Guid id);
    public Task<PaginationResult<ObjectiveModel>> SearchAsync(string? name, Page page);
}