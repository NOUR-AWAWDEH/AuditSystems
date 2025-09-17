using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetById;

public sealed record class GetRiskByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetRiskByIdQueryResponse>>;