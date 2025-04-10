using AuditSystem.Application.Constants;
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
    private static readonly string[] JobSchedulingTags = ["job-schedulings", "job-scheduling-list"];
    private static readonly string[] ListTags = ["job-scheduling-list"]; // Tags for collections only

    public async Task<Guid> CreateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel)
    {
        ArgumentNullException.ThrowIfNull(jobSchedulingModel, nameof(jobSchedulingModel));

        try
        {
            var entity = mapper.Map<JobScheduling>(jobSchedulingModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.JobScheduling, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: JobSchedulingTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create JobScheduling.", ex);
        }
    }
}