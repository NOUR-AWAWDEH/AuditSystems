using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetById;

public sealed record GetAuditProgramByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetAuditProgramByIdQueryResponse>>;
