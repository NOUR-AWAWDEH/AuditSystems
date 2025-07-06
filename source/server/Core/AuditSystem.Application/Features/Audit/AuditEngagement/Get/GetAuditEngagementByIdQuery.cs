using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Get;

public sealed record class GetAuditEngagementByIdQuery(Guid Id) : IRequest<Result<GetAuditEngagementByIdQueryResponse>>;