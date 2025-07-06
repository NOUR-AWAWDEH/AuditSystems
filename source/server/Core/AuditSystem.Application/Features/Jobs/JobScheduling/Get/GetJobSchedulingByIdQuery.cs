using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Get;

public sealed record class GetJobSchedulingByIdQuery(Guid Id) : IRequest<Result<GetJobSchedulingByIdQueryResponse>>;