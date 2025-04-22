using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Get;

public sealed record class GetAuditJobByIdQuery(Guid Id): IRequest<Result<GetAuditJobByIdQueryResponse>>;