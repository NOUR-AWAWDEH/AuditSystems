using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetById;

public sealed record GetAuditProcessByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetAuditProcessByIdQueryResponse>>;