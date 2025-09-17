using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetByUserId;

public sealed record GetSubLocationsByUserIdQuery : UserAwareQuery<Result<GetSubLocationsByUserIdQueryResponse>>;