using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Pagination;
namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;

public interface IObjectiveService
{
    Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel, Guid userId);
    Task UpdateObjectiveAsync(ObjectiveModel objectiveModel, Guid userId);
    Task DeleteObjectiveAsync(Guid objectiveId, Guid userId);
    Task<ObjectiveModel> GetObjectiveByIdAsync(Guid id, Guid userId);
    Task<ICollection<ObjectiveModel>> GetObjectivesByUserIdAsync(Guid userId);
    Task<PaginationResult<ObjectiveModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid riskControlMatrixId, Page? page);

    // The Exiting SearchAsync for admin/system use if needed
    Task<PaginationResult<ObjectiveModel>> SearchAsync(string? description, Page page);
}