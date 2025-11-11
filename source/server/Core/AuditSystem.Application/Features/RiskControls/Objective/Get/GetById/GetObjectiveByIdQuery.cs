using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetById;

public sealed record class GetObjectiveByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetObjectiveByIdQueryResponse>>;