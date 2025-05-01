using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Get;

public sealed record class GetAuditDomainByIdQuery(Guid Id) : IRequest<Result<GetAuditDomainByIdQueryResponse>>;
