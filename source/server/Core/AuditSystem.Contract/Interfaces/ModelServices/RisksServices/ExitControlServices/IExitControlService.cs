using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

public interface IExitControlService
{
    Task<Guid> CreateExitControlAsync(ExitControlModel exitControlModel, Guid userId);
    Task UpdateExitControlAsync(ExitControlModel exitControlModel, Guid userId);
    Task DeleteExitControlAsync(Guid id, Guid userId);
    Task<ExitControlModel> GetExitControlByIdAsync(Guid id, Guid userId);

    Task<IReadOnlyList<ExitControlModel>> GetExitControlsByUserIdAsync(Guid userId);
    Task<PaginationResult<ExitControlModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid riskId, Page? page);
    Task<PaginationResult<ExitControlModel>> SearchAsync(string? name, Page page);
}
