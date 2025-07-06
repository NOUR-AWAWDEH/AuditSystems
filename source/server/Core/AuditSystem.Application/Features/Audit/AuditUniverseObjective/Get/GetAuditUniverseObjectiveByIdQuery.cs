using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Get;

public sealed record class GetAuditUniverseObjectiveByIdQuery(Guid Id) : IRequest<Result<GetAuditUniverseObjectiveByIdQueryResponse>>;