using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Get;

public sealed record class GetJobTimeAllocationReportByIdQuery(Guid Id) : IRequest<Result<GetJobTimeAllocationReportByIdQueryResponse>>;