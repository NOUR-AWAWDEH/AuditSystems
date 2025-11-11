using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetByUserId;

public sealed record GetAuditProgramsByUserIdQuery : UserAwareQuery<Result<GetAuditProgramsByUserIdQueryResponse>>;
