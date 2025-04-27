
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Get;
    
public sealed record class GetJobSchedulingByIdQueryResponse(JobSchedulingModel JobSchedulingModel);