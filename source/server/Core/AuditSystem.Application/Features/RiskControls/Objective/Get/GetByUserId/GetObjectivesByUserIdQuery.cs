using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetByUserId;

public sealed record GetObjectivesByUserIdQuery : UserAwareQuery<Result<GetObjectivesByUserIdQueryResponse>>;
