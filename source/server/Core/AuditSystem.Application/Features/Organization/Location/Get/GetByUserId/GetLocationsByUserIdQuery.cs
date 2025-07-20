using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

public sealed record GetLocationsByUserIdQuery : UserAwareQuery<Result<GetLocationsByUserIdQueryResponse>>;