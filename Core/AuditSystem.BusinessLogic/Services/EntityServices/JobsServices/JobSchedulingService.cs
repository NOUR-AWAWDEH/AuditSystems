using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class JobSchedulingService(
    IRepository<Guid, JobScheduling> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobSchedulingService> logger) : 
    BaseEntityCacheService<JobScheduling>(
        cacheService,
        logger,
        CacheKeys.JobScheduling,
        JobSchedulingTags,
        ListTags
    ),
    IJobSchedulingService
{
    private static readonly string[] JobSchedulingTags = ["job-schedulings", "job-scheduling-list"];
    private static readonly string[] ListTags = ["job-scheduling-list"];

    public async Task<Guid> CreateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel)
    {
        ArgumentNullException.ThrowIfNull(jobSchedulingModel, nameof(jobSchedulingModel));

        try
        {
            var entity = mapper.Map<JobScheduling>(jobSchedulingModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for JobSchedulingModel");
            throw new ArgumentException("Mapping failed for JobSchedulingModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the JobScheduling.");
            throw new InvalidOperationException("An error occurred while creating the JobScheduling.", ex);
        }
    }

    public async Task DeleteJobSchedulingAsync(Guid jobSchedulingId)
    {
        var cacheKey = GetCacheKey(jobSchedulingId);
        
        try
        {
            var entity = await repository.GetByIdAsync(jobSchedulingId)
                ?? throw new KeyNotFoundException($"JobScheduling with ID {jobSchedulingId} not found.");

            await repository.DeleteAsync(jobSchedulingId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "JobScheduling with ID {JobSchedulingId} not found", jobSchedulingId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete JobScheduling ID {JobSchedulingId}", jobSchedulingId);
            throw new InvalidOperationException($"Failed to delete JobScheduling: {jobSchedulingId}", ex);
        }
    }

    public async Task<JobSchedulingModel> GetJobSchedulingByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<JobScheduling>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<JobSchedulingModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"JobScheduling with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<JobSchedulingModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve JobScheduling with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve JobScheduling with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<JobSchedulingModel>> SearchAsync(string? name, Page page)
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

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update JobScheduling with ID {Id}", jobSchedulingModel.Id);
            throw new InvalidOperationException($"Failed to update JobScheduling with ID '{jobSchedulingModel.Id}'", ex);
        }
    }
}