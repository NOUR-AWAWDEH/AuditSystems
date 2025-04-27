using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Get;

public sealed record class GetBusinessObjectiveByIdQuery(Guid Id) : IRequest<Result<GetBusinessObjectiveByIdQueryResponse>>;