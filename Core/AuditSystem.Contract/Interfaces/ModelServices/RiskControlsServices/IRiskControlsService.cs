using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskControlsService
{
    public Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel);
}
