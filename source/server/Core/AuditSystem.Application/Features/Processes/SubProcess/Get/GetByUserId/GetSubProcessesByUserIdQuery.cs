using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetByUserId;

public sealed record GetSubProcessesByUserIdQuery: UserAwareQuery<Result<GetSubProcessesByUserIdQueryResponse>>;
