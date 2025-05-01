using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditUniverseService> logger):
    UnifiedCacheService<Guid, AuditUniverse, AuditUniverseModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditUniverse,
        AuditUniverseTags,
        ListTags
    ),
    IAuditUniverseService
{
    private static readonly string[] AuditUniverseTags = ["audit-universes", "audit-universe-list"];
    private static readonly string[] ListTags = ["audit-universe-list"];

    public async Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);

        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteAuditUniverseAsync(Guid auditUniverseId)
    {
        var cacheKey = GetCacheKey(auditUniverseId);
       
        var entity = await repository.GetByIdAsync(auditUniverseId)
                        ?? throw new KeyNotFoundException($"AuditUniverse with ID {auditUniverseId} not found.");

        await repository.DeleteAsync(auditUniverseId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<AuditUniverseModel> GetAuditUniverseByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var cachedEntity = await cacheService.GetAsync<AuditUniverse>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditUniverseModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException($"AuditUniverse with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);

        return mapper.Map<AuditUniverseModel>(entity);
    }

    public async Task<PaginationResult<AuditUniverseModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditUniverseModel>(cacheKey, countCacheKey, page);
        if (cachedResult!= null)
            return cachedResult;
        
        return await GetFromRepositoryAsync<AuditUniverseModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));

        var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
