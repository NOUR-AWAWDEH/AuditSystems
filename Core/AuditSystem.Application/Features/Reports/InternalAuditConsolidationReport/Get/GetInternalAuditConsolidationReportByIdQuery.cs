using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Get;

public sealed record class GetInternalAuditConsolidationReportByIdQuery(Guid Id) : IRequest<Result<GetInternalAuditConsolidationReportByIdQueryResponse>>;