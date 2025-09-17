using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetById;


public sealed record class GetSubProcessByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetSubProcessByIdQueryResponse>>;