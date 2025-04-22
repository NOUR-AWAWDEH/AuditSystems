using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Get;

public sealed record class GetAuditPlanSummaryReportByIdQuery(Guid Id) : IRequest<Result<GetAuditPlanSummaryReportByIdQueryResponse>>;