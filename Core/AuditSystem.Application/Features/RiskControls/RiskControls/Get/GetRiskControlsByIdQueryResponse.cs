using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Get;

public sealed record class GetRiskControlsByIdQueryResponse(RiskControlsModel RiskControlsModel);