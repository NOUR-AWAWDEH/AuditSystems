using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetById;

public sealed record GetSubLocationByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetSubLocationByIdQueryResponse>>;