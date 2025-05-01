using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

public sealed class AuditDomainService(
    IRepository<Guid, AuditDomain> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditDomainService> logger)
    :
    UnifiedCacheService<Guid, AuditDomain, AuditDomainModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditDomain,
        AuditDomainTags,
        ListTags
    )
    , IAuditDomainService
{
    private static readonly string[] AuditDomainTags = ["audit-domains", "audit-domain-list"];
    private static readonly string[] ListTags = ["audit-domain-list"];

    public async Task<Guid> CreateAuditDomainAsync(AuditDomainModel auditDomainModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomainModel, nameof(auditDomainModel));

        var entity = mapper.Map<AuditDomain>(auditDomainModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteAuditDomainAsync(Guid auditDomainId)
    {
        var cacheKey = GetCacheKey(auditDomainId);

        var entity = await repository.GetByIdAsync(auditDomainId)
                     ?? throw new KeyNotFoundException($"AuditDomain with ID {auditDomainId} not found.");

        await repository.DeleteAsync(auditDomainId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<AuditDomainModel> GetAuditDomainByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        var cachedEntity = await cacheService.GetAsync<AuditDomain>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditDomainModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
                     ?? throw new KeyNotFoundException($"AuditDomain with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);

        return mapper.Map<AuditDomainModel>(entity);
    }

    public async Task<PaginationResult<AuditDomainModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditDomainModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditDomainModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditDomainAsync(AuditDomainModel auditDomainModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomainModel, nameof(auditDomainModel));

        var entity = mapper.Map<AuditDomain>(auditDomainModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}