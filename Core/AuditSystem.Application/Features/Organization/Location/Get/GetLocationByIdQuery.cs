using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Location.Get;

public sealed record class GetLocationByIdQuery(Guid Id) : IRequest<Result<GetLocationByIdQueryResponse>>;