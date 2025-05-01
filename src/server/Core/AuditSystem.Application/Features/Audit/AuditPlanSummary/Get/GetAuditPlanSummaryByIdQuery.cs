using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Get;

public sealed record class GetAuditPlanSummaryByIdQuery(Guid Id) : IRequest<Result<GetAuditPlanSummaryByIdQueryResponse>>;