using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskService
{
    Task<Guid> CreateRiskAsync(RiskModel riskModel, Guid userId);
    Task UpdateRiskAsync(RiskModel riskModel, Guid userId);
    Task DeleteRiskAsync(Guid riskId, Guid userId);
    Task<RiskModel> GetRiskByIdAsync(Guid id, Guid userId);
    Task<IReadOnlyList<RiskModel>> GetRisksByUserIdAsync(Guid userId);

    Task<PaginationResult<RiskModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid observationId, Page? page);
    // The existing SearchAsync for admin/system use if needed
    Task<PaginationResult<RiskModel>> SearchAsync(string? name, Page page);
}