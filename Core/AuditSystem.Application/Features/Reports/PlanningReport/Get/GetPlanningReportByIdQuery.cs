using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Get;

public sealed record class GetPlanningReportByIdQuery(Guid Id) : IRequest<Result<GetPlanningReportByIdQueryResponse>>;
