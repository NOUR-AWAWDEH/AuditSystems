using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetByUserId;


public sealed record class GetRisksByUserIdQuery : UserAwareQuery<Result<GetRisksByUserIdQueryResponse>>;
