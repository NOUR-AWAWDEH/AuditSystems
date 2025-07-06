using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Get;

public sealed record class GetJobPrioritizationByIdQuery(Guid Id) : IRequest<Result<GetJobPrioritizationByIdQueryResponse>>;