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

internal sealed class JobSchedulingService(
    IRepository<Guid, JobScheduling> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobSchedulingService> logger) : 
    UnifiedCacheService<Guid, JobScheduling, JobSchedulingModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<JobScheduling>(jobSchedulingModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteJobSchedulingAsync(Guid jobSchedulingId)
    {
        var cacheKey = GetCacheKey(jobSchedulingId);
        
        var entity = await repository.GetByIdAsync(jobSchedulingId)
            ?? throw new KeyNotFoundException($"JobScheduling with ID {jobSchedulingId} not found.");

        await repository.DeleteAsync(jobSchedulingId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<JobSchedulingModel> GetJobSchedulingByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<JobScheduling>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<JobSchedulingModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"JobScheduling with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<JobSchedulingModel>(entity);
    }

    public async Task<PaginationResult<JobSchedulingModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<JobSchedulingModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<JobSchedulingModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateJobSchedulingAsync(JobSchedulingModel jobSchedulingModel)
    {
        ArgumentNullException.ThrowIfNull(jobSchedulingModel, nameof(jobSchedulingModel));

        var entity = mapper.Map<JobScheduling>(jobSchedulingModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}