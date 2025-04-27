using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

public sealed record class GetSubLocationByIdQuery(Guid Id) : IRequest<Result<GetSubLocationByIdQueryResponse>>;