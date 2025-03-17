using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class JobPrioritizationService(
    IRepository<Guid, JobPrioritization> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IJobPrioritizationService
{
    public Task<Guid> CreateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        throw new NotImplementedException();
    }
}