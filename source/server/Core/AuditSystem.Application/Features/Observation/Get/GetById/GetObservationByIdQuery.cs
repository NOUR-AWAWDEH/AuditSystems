using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Get.GetById;

public sealed record GetObservationByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetObservationByIdQueryResponse>>;
