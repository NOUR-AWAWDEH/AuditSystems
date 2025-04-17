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

    public async Task DeleteJobSchedulingAsync(Guid jobSchedulingId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(jobSchedulingId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"JobScheduling with ID {jobSchedulingId} not found.");
            }

            await repository.DeleteAsync(jobSchedulingId);

            var cacheKey = string.Format(CacheKeys.JobScheduling, jobSchedulingId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete JobScheduling.", ex);
        }
    }

    public Task<JobSchedulingModel> GetJobSchedulingByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel)
    {
        ArgumentNullException.ThrowIfNull(jobSchedulingModel, nameof(jobSchedulingModel));

        try
        {
            var entity = mapper.Map<JobScheduling>(jobSchedulingModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.JobScheduling, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: JobSchedulingTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update JobScheduling.", ex);
        }
    }
}