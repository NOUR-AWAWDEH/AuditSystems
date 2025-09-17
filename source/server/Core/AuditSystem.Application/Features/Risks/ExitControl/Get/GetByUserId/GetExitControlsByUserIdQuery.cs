using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetByUserId;


public sealed record class GetExitControlsByUserIdQuery : UserAwareQuery<Result<GetExitControlsByUserIdQueryResponse>>;
