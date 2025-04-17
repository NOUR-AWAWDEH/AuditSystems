using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IObjectiveService
{
    public Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel);
    public Task UpdateObjectiveAsync(ObjectiveModel objectiveModel);
    public Task DeleteObjectiveAsync(Guid objectiveId);
    public Task<ObjectiveModel> GetObjectiveByIdAsync(Guid Id);
}