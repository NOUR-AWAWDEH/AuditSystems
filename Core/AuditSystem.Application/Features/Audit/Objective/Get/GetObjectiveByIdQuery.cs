using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Audit.Objective.Get;

public sealed record class GetObjectiveByIdQuery(Guid Id) : IRequest<Result<GetObjectiveByIdQueryResponse>>;