using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetByUserId;

public sealed record GetRiskControlMatricesByUserIdQueryResponse(IReadOnlyList<RiskControlMatrixModel> RiskControlMatrixModels);
