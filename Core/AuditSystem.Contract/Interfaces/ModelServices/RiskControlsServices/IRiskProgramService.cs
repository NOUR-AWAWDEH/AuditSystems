using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;

public interface IRiskProgramService
{
    public Task<Guid> CreateRiskProgramAsync(RiskProgramModel riskProgramModel);
    public Task UpdateRiskProgramAsync(RiskProgramModel riskProgramModel);
    public Task DeleteRiskProgramAsync(Guid riskProgramId);
}
