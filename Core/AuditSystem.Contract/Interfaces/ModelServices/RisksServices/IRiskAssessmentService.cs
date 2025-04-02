using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskAssessmentService
{
    public Task<Guid> CreateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel);
}