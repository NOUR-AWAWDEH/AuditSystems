using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class JobPrioritizationService(
    IRepository<Guid, JobPrioritization> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobPrioritizationService> logger) :
    UnifiedCacheService<Guid, JobPrioritization, JobPrioritizationModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.JobPrioritization,
        JobPrioritizationTags,
        ListTags
    ),
    IJobPrioritizationService
{
    private static readonly string[] JobPrioritizationTags = ["job-prioritizations", "job-prioritization-list"];
    private static readonly string[] ListTags = ["job-prioritization-list"];
    
    public async Task<Guid> CreateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        ArgumentNullException.ThrowIfNull(jobPrioritizationModel, nameof(jobPrioritizationModel));

        var entity = mapper.Map<JobPrioritization>(jobPrioritizationModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteJobPrioritizationAsync(Guid jobPrioritizationId)
    {
        var cacheKey = GetCacheKey(jobPrioritizationId);
        
        var entity = await repository.GetByIdAsync(jobPrioritizationId)
            ?? throw new KeyNotFoundException($"JobPrioritization with ID {jobPrioritizationId} not found.");

        await repository.DeleteAsync(jobPrioritizationId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<JobPrioritizationModel> GetJobPrioritizationByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var cachedEntity = await cacheService.GetAsync<JobPrioritization>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<JobPrioritizationModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"JobPrioritization with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<JobPrioritizationModel>(entity);
    }

    public async Task<PaginationResult<JobPrioritizationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<JobPrioritizationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<JobPrioritizationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateJobPrioritizationAsync(JobPrioritizationModel jobPrioritizationModel)
    {
        ArgumentNullException.ThrowIfNull(jobPrioritizationModel, nameof(jobPrioritizationModel));

        var entity = mapper.Map<JobPrioritization>(jobPrioritizationModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}