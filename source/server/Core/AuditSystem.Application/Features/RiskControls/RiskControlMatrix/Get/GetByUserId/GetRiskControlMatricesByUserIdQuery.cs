using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetByUserId;

public sealed record GetRiskControlMatricesByUserIdQuery : UserAwareQuery<Result<GetRiskControlMatricesByUserIdQueryResponse>>;