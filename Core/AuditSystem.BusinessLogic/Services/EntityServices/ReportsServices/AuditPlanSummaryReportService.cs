using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class AuditPlanSummaryReportService(
    IRepository<Guid, AuditPlanSummaryReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditPlanSummaryReportService> logger) : 
    BaseEntityCacheService<AuditPlanSummaryReport>(
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

        try
        {
            var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditPlanSummaryReportModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditPlanSummaryReport");
            throw new InvalidOperationException("Failed to create AuditPlanSummaryReport.", ex);
        }
    }
    
    public async Task DeleteAuditPlanSummaryReportAsync(Guid auditPlanSummaryReportId)
    {
        var cacheKey = GetCacheKey(auditPlanSummaryReportId);
        
        try
        {
            var entity = await repository.GetByIdAsync(auditPlanSummaryReportId)
                ?? throw new KeyNotFoundException($"AuditPlanSummaryReport with ID {auditPlanSummaryReportId} not found.");

            await repository.DeleteAsync(auditPlanSummaryReportId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "AuditPlanSummaryReport with ID {ReportId} not found", auditPlanSummaryReportId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditPlanSummaryReport ID {ReportId}", auditPlanSummaryReportId);
            throw new InvalidOperationException($"Failed to delete AuditPlanSummaryReport: {auditPlanSummaryReportId}", ex);
        }
    }
    
    public async Task<AuditPlanSummaryReportModel> GetAuditPlanSummaryReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditPlanSummaryReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditPlanSummaryReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"AuditPlanSummaryReport with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<AuditPlanSummaryReportModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditPlanSummaryReport with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditPlanSummaryReport with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditPlanSummaryReport with ID {Id}", auditPlanSummaryReportModel.Id);
            throw new InvalidOperationException($"Failed to update AuditPlanSummaryReport with ID '{auditPlanSummaryReportModel.Id}'", ex);
        }
    }
}
