using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;


public interface IRiskControlMatrixService
{
    Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel, Guid userId);
    Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel, Guid userId);
    Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId, Guid userId);
    Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id, Guid userId);
    Task<ICollection<RiskControlMatrixModel>> GetRiskControlMatricesByUserIdAsync(Guid userId);
    Task<PaginationResult<RiskControlMatrixModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid SubProcessId, Page? page);

    // The Exiting SearchAsync for admin/system use if needed
    Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? description, Page page);
}