using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class JobSchedulingService(
    IRepository<Guid, JobScheduling> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IJobSchedulingService
{
    public Task<Guid> CreateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel)
    {
        throw new NotImplementedException();
    }
}