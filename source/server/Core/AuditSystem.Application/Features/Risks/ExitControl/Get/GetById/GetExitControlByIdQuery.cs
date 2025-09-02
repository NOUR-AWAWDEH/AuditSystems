using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetById;

public sealed record class GetExitControlByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetExitControlByIdQueryResponse>>;