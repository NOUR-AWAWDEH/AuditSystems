using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

public sealed record class GetSubLocationByIdQuery : IRequest<Result<GetSubLocationByIdQueryResponse>>
{
    public required Guid Id { get; init; }
    public Guid UserId { get; init; }
}