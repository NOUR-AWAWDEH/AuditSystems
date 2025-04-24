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

internal sealed class AuditPlanSummaryService(
    IRepository<Guid, AuditPlanSummary> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditPlanSummaryService> logger) : 
    UnifiedCacheService<Guid, AuditPlanSummary, AuditPlanSummaryModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditPlanSummary,
        AuditPlanSummaryListKeys,
        ListTags
    ),
    IAuditPlanSummaryService
{
    private static readonly string[] AuditPlanSummaryListKeys = ["audit-plan-summaries", "audit-plan-summary-list"];
    private static readonly string[] ListTags = ["audit-plan-summary-list"];

    public async Task<Guid> CreateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));
        
        var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteAuditPlanSummaryAsync(Guid auditPlanSummaryId)
    {
        var cacheKey = GetCacheKey(auditPlanSummaryId);

        var entity = await repository.GetByIdAsync(auditPlanSummaryId) ?? 
                throw new KeyNotFoundException($"AuditPlanSummary with ID {auditPlanSummaryId} not found.");

        await repository.DeleteAsync(auditPlanSummaryId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<AuditPlanSummaryModel> GetAuditPlanSummaryByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        var cachedEntity = await cacheService.GetAsync<AuditPlanSummary>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditPlanSummaryModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id) 
                        ?? throw new KeyNotFoundException($"AuditPlanSummary with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);

        return mapper.Map<AuditPlanSummaryModel>(entity);
    }

    public Task<PaginationResult<AuditPlanSummaryModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));

        var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}