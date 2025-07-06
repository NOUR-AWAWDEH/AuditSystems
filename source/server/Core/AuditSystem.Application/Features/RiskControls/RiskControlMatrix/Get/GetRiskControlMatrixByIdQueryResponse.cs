using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get;

public sealed record class GetRiskControlMatrixByIdQueryResponse(RiskControlMatrixModel RiskControlMatrixModel);
