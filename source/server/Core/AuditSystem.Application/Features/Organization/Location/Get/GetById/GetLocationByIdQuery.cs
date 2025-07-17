using Ardalis.Result;
using AuditSystem.Application.Base;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetById;

public sealed record GetLocationByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetLocationByIdQueryResponse>>;