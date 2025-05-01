using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskFactorService
{
    public Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel);
    public Task UpdateRiskFactorAsync(RiskFactorModel riskFactorModel);
    public Task DeleteRiskFactorAsync(Guid riskFactorId);
    public Task<RiskFactorModel> GetRiskFactorByIdAsync(Guid id);
    public Task<PaginationResult<RiskFactorModel>> SearchAsync(string? name, Page page);
}