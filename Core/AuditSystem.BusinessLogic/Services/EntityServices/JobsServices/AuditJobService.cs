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

internal sealed class AuditJobService(
    IRepository<Guid, AuditJob> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditJobService> logger) :
    UnifiedCacheService<Guid, AuditJob, AuditJobModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditJob,
        AuditJobTags,
        ListTags
    ),
    IAuditJobService
{
    private static readonly string[] AuditJobTags = ["audit-jobs", "audit-job-list"];
    private static readonly string[] ListTags = ["audit-job-list"];

    public async Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));

        var entity = mapper.Map<AuditJob>(auditJobModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id; 
    }
    
    public async Task DeleteAuditJobAsync(Guid auditJobId)
    {
        var cacheKey = GetCacheKey(auditJobId);

        var entity = await repository.GetByIdAsync(auditJobId)
            ?? throw new KeyNotFoundException($"AuditJob with ID {auditJobId} not found.");

        await repository.DeleteAsync(auditJobId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<AuditJobModel> GetAuditJobByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<AuditJob>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditJobModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"AuditJob with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<AuditJobModel>(entity);
    }

    public async Task<PaginationResult<AuditJobModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditJobModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditJobModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));

        var entity = mapper.Map<AuditJob>(auditJobModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}