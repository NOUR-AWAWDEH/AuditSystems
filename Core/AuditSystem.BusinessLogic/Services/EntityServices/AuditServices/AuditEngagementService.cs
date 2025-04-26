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

internal sealed class AuditEngagementService(
    IRepository<Guid, AuditEngagement> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditEngagementService> logger
) : UnifiedCacheService<Guid, AuditEngagement, AuditEngagementModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditEngagement,
        AuditEngagementTags,
        ListTags
    ),
    IAuditEngagementService
{
    private static readonly string[] AuditEngagementTags = ["audit-engagements", "audit-engagment-list" ];
    private static readonly string[] ListTags = ["audit-engagement-list"];

    public async Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));

        var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteAuditEngagementAsync(Guid auditEngagementId)
    {
        var cacheKey = GetCacheKey(auditEngagementId);

        var entity = await repository.GetByIdAsync(auditEngagementId)
                ?? throw new KeyNotFoundException($"AuditEngagement with ID '{auditEngagementId}' not found.");
        
        await repository.DeleteAsync(auditEngagementId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<AuditEngagementModel> GetAuditEngagementByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        var cachedEntity = await cacheService.GetAsync<AuditEngagement>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditEngagementModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
                        ?? throw new KeyNotFoundException($"AuditEngagement with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);

        return mapper.Map<AuditEngagementModel>(entity);
    }

    public async Task<PaginationResult<AuditEngagementModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        var(cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditEngagementModel>(cacheKey, countCacheKey, page);
        if (cachedResult!= null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditEngagementModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));

        var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
