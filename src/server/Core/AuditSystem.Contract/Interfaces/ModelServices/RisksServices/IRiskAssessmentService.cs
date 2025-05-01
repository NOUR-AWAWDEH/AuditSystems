using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskAssessmentService
{
    public Task<Guid> CreateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel);
    public Task UpdateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel);
    public Task DeleteRiskAssessmentAsync(Guid riskAssessmentId);
    public Task<RiskAssessmentModel> GetRiskAssessmentByIdAsync(Guid id);
    public Task<PaginationResult<RiskAssessmentModel>> SearchAsync(string? name, Page page);
}