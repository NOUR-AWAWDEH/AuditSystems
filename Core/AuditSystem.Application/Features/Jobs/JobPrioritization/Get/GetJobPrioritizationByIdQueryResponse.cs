using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Get;

public sealed record class GetJobPrioritizationByIdQueryResponse(JobPrioritizationModel JobPrioritizationModel);