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

internal sealed class AuditPlanSummaryReportService(
    IRepository<Guid, AuditPlanSummaryReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditPlanSummaryReportService> logger) : 
    UnifiedCacheService<Guid, AuditPlanSummaryReport, AuditPlanSummaryReportModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditPlanSummaryReport,
        AuditPlanSummaryReportTags,
        ListTags
    ),
    IAuditPlanSummaryReportService
{
    private static readonly string[] AuditPlanSummaryReportTags = ["audit-plan-summary-reports", "audit-plan-summary-report-list"];
    private static readonly string[] ListTags = ["audit-plan-summary-report-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));

        var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteAuditPlanSummaryReportAsync(Guid auditPlanSummaryReportId)
    {
        var cacheKey = GetCacheKey(auditPlanSummaryReportId);
        
        var entity = await repository.GetByIdAsync(auditPlanSummaryReportId)
            ?? throw new KeyNotFoundException($"AuditPlanSummaryReport with ID {auditPlanSummaryReportId} not found.");

        await repository.DeleteAsync(auditPlanSummaryReportId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<AuditPlanSummaryReportModel> GetAuditPlanSummaryReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<AuditPlanSummaryReport>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditPlanSummaryReportModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"AuditPlanSummaryReport with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<AuditPlanSummaryReportModel>(entity);
    }

    public async Task<PaginationResult<AuditPlanSummaryReportModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditPlanSummaryReportModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<AuditPlanSummaryReportModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));

        var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
