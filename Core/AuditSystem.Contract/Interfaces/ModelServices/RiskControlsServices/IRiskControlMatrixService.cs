using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskControlMatrixService
{
    public Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel);
    public Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel);
    public Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId);
    public Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id);
    public Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? name, Page page);
}
