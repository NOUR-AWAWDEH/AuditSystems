using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskFactorService
{
    public Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel);
}