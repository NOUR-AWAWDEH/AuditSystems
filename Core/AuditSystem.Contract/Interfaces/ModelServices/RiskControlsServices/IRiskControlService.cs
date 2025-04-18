using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskControlService
{
    public Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel);
    public Task UpdateRiskControlAsync(RiskControlsModel riskControlModel);
    public Task DeleteRiskControlAsync(Guid riskControlId);
    public Task<RiskControlsModel> GetRiskControlByIdAsync(Guid id);
}
