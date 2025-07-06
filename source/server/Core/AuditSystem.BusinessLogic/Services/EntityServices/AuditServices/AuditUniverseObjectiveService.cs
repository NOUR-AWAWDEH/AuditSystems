using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseObjectiveService(
    IRepository<Guid, AuditUniverseObjective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditUniverseObjectiveService> logger) :
    UnifiedCacheService<Guid, AuditUniverseObjective, AuditUniverseObjectiveModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditUniverseObjective,
        AuditUniverseObjectiveTags,
        ListTags
    ),
    IAuditUniverseObjectiveService

{
    private static readonly string[] AuditUniverseObjectiveTags = ["audit-universe-objectives", "audit-universe-objective-list"];
    private static readonly string[] ListTags = ["audit-universe-objective-list"];

    public async Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));
       
        var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }

    public async Task DeleteAuditUniverseObjectiveAsync(Guid auditUniverseObjectiveId)
    {
        var cacheKey = GetCacheKey(auditUniverseObjectiveId);
        
        var entity = await repository.GetByIdAsync(auditUniverseObjectiveId)
            ?? throw new KeyNotFoundException($"AuditUniverseObjective with ID {auditUniverseObjectiveId} not found.");

        await repository.DeleteAsync(auditUniverseObjectiveId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<AuditUniverseObjectiveModel> GetAuditUniverseObjectiveByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var cachedEntity = await cacheService.GetAsync<AuditUniverseObjective>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditUniverseObjectiveModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException($"AuditUniverseObjective with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);

        return mapper.Map<AuditUniverseObjectiveModel>(entity);
    }

    public async Task<PaginationResult<AuditUniverseObjectiveModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
    
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);
        
        var cachedResult = await GetFromCacheAsync<AuditUniverseObjectiveModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditUniverseObjectiveModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));
        
        var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
