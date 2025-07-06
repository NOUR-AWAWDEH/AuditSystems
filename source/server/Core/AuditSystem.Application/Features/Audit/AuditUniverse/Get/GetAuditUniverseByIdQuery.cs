using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Get;

public sealed record class GetAuditUniverseByIdQuery(Guid Id) : IRequest<Result<GetAuditUniverseByIdQueryResponse>>;