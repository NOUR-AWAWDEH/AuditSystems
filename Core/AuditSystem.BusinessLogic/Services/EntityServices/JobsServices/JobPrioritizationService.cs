using AuditSystem.Application.Constants;
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
    private static readonly string[] JobPrioritizationTags = ["job-prioritizations", "job-prioritization-list"];
    private static readonly string[] ListTags = ["job-prioritization-list"]; // Tags for collections only
    public async Task<Guid> CreateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        ArgumentNullException.ThrowIfNull(jobPrioritizationModel, nameof(jobPrioritizationModel));

        try
        {
            var entity = mapper.Map<JobPrioritization>(jobPrioritizationModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.JobPrioritization, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: JobPrioritizationTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create JobPrioritization.", ex);
        }
    }
    public async Task DeleteJobPrioritizationAsync(Guid jobPrioritizationId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(jobPrioritizationId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"JobPrioritization with ID {jobPrioritizationId} not found.");
            }

            await repository.DeleteAsync(jobPrioritizationId);

            var cacheKey = string.Format(CacheKeys.JobPrioritization, jobPrioritizationId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete JobPrioritization.", ex);
        }
    }
    public async Task<JobPrioritizationModel> GetJobPrioritizationByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.JobPrioritization, id);

            var cachedEntity = await cacheService.GetAsync<JobPrioritization>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<JobPrioritizationModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"JobPrioritization with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: JobPrioritizationTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<JobPrioritizationModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve JobPrioritization by ID {id}.", ex);
        }
    }
    public async Task UpdateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        ArgumentNullException.ThrowIfNull(jobPrioritizationModel, nameof(jobPrioritizationModel));

        try
        {
            var entity = mapper.Map<JobPrioritization>(jobPrioritizationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.JobPrioritization, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: JobPrioritizationTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update JobPrioritization.", ex);
        }
    }
}