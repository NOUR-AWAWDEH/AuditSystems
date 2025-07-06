using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class InternalAuditConsolidationReportService(
    IRepository<Guid, InternalAuditConsolidationReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<InternalAuditConsolidationReportService> logger) : 
    UnifiedCacheService<Guid, InternalAuditConsolidationReport, InternalAuditConsolidationReportModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.InternalAuditConsolidationReport,
        InternalAuditConsolidationReportTags,
        ListTags
    ),
    IInternalAuditConsolidationReportService
{
    private static readonly string[] InternalAuditConsolidationReportTags = ["internal-audit-consolidation-reports", "internal-audit-consolidation-report-list"];
    private static readonly string[] ListTags = ["internal-audit-consolidation-report-list"];

    public async Task<Guid> CreateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));

        var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteInternalAuditConsolidationReportAsync(Guid internalAuditConsolidationReportId)
    {
        var cacheKey = GetCacheKey(internalAuditConsolidationReportId);
        
        var entity = await repository.GetByIdAsync(internalAuditConsolidationReportId)
            ?? throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID {internalAuditConsolidationReportId} not found.");

        await repository.DeleteAsync(internalAuditConsolidationReportId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<InternalAuditConsolidationReportModel> GetInternalAuditConsolidationReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<InternalAuditConsolidationReport>(cacheKey);
        if (entity != null)
            return mapper.Map<InternalAuditConsolidationReportModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<InternalAuditConsolidationReportModel>(entity);
    }

    public async Task<PaginationResult<InternalAuditConsolidationReportModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<InternalAuditConsolidationReportModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<InternalAuditConsolidationReportModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));
        
        var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
