using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskService
{
    public Task<Guid> CreateRiskAsync(RiskModel riskModel);
}