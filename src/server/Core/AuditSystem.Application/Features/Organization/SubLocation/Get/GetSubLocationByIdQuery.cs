using Ardalis.Result;
using AuditSystem.Application.Base;
using MediatR;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

public sealed record GetSubLocationByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetSubLocationByIdQueryResponse>>;