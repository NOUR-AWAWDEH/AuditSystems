using Ardalis.Result;
using AuditSystem.Application.Base;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

public sealed record GetSubLocationsByUserIdQuery : UserAwareQuery<Result<GetSubLocationsByUserIdQueryResponse>>;