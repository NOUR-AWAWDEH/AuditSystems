using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetByUserId;

public sealed record GetAuditProcessesByUserIdQuery: UserAwareQuery<Result<GetAuditProcessesByUserIdQueryResponse>>;