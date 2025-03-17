using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskControlService
{
    public Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel);
}
