using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Get;

public sealed record class GetAuditExceptionReportByIdQuery(Guid Id) : IRequest<Result<GetAuditExceptionReportByIdQueryResponse>>;