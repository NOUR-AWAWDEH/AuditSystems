using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskControlMatrixService
{
    public Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel);
}
