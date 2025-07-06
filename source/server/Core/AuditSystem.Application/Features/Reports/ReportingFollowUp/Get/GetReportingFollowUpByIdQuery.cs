using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Get;

public sealed record class GetReportingFollowUpByIdQuery(Guid Id) : IRequest<Result<GetReportingFollowUpByIdQueryResponse>>;