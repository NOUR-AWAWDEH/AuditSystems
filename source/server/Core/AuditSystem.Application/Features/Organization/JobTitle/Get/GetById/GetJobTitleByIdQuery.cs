using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetById;

public sealed record GetJobTitleByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetJobTitleByIdQueryResponse>>;