using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetByUserId;

public sealed record GetRisksByUserIdQueryResponse(IReadOnlyList<RiskModel> RiskModels);