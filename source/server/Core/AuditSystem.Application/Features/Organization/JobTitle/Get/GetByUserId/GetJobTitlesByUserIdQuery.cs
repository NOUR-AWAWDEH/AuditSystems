using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetByUserId;

public sealed record GetJobTitlesByUserIdQuery : UserAwareQuery<Result<GetJobTitlesByUserIdQueryResponse>>;
