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

internal sealed class JobPrioritizationService(
    IRepository<Guid, JobPrioritization> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobPrioritizationService> logger) :
    BaseEntityCacheService<JobPrioritization>(
        cacheService,
        logger,
        CacheKeys.JobPrioritization,
        JobPrioritizationTags,
        ListTags
    ),
    IJobPrioritizationService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for JobPrioritizationModel");
            throw new ArgumentException("Mapping failed for JobPrioritizationModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the JobPrioritization.");
            throw new InvalidOperationException("An error occurred while creating the JobPrioritization.", ex);
        }
    }
    
    public async Task DeleteJobPrioritizationAsync(Guid jobPrioritizationId)
    {
        var cacheKey = GetCacheKey(jobPrioritizationId);
        
        try
        {
            var entity = await repository.GetByIdAsync(jobPrioritizationId)
                ?? throw new KeyNotFoundException($"JobPrioritization with ID {jobPrioritizationId} not found.");

            await repository.DeleteAsync(jobPrioritizationId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "JobPrioritization with ID {JobPrioritizationId} not found", jobPrioritizationId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete JobPrioritization ID {JobPrioritizationId}", jobPrioritizationId);
            throw new InvalidOperationException($"Failed to delete JobPrioritization: {jobPrioritizationId}", ex);
        }
    }
    
    public async Task<JobPrioritizationModel> GetJobPrioritizationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<JobPrioritization>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<JobPrioritizationModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"JobPrioritization with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<JobPrioritizationModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve JobPrioritization with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve JobPrioritization with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<JobPrioritizationModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        ArgumentNullException.ThrowIfNull(jobPrioritizationModel, nameof(jobPrioritizationModel));

        try
        {
            var entity = mapper.Map<JobPrioritization>(jobPrioritizationModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update JobPrioritization with ID {Id}", jobPrioritizationModel.Id);
            throw new InvalidOperationException($"Failed to update JobPrioritization with ID '{jobPrioritizationModel.Id}'", ex);
        }
    }
}