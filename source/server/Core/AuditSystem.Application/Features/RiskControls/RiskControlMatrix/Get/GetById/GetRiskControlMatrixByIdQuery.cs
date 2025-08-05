using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetById;

public sealed record class GetRiskControlMatrixByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetRiskControlMatrixByIdQueryResponse>>;