using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class InternalAuditConsolidationReportService(
    IRepository<Guid, InternalAuditConsolidationReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<InternalAuditConsolidationReportService> logger) : 
    BaseEntityCacheService<InternalAuditConsolidationReport>(
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

        try
        {
            var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for InternalAuditConsolidationReportModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create InternalAuditConsolidationReport");
            throw new InvalidOperationException("Failed to create InternalAuditConsolidationReport.", ex);
        }
    }
    
    public async Task DeleteInternalAuditConsolidationReportAsync(Guid internalAuditConsolidationReportId)
    {
        var cacheKey = GetCacheKey(internalAuditConsolidationReportId);
        
        try
        {
            var entity = await repository.GetByIdAsync(internalAuditConsolidationReportId)
                ?? throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID {internalAuditConsolidationReportId} not found.");

            await repository.DeleteAsync(internalAuditConsolidationReportId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "InternalAuditConsolidationReport with ID {ReportId} not found", internalAuditConsolidationReportId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete InternalAuditConsolidationReport ID {ReportId}", internalAuditConsolidationReportId);
            throw new InvalidOperationException($"Failed to delete InternalAuditConsolidationReport: {internalAuditConsolidationReportId}", ex);
        }
    }
    
    public async Task<InternalAuditConsolidationReportModel> GetInternalAuditConsolidationReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<InternalAuditConsolidationReport>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<InternalAuditConsolidationReportModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<InternalAuditConsolidationReportModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve InternalAuditConsolidationReport with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve InternalAuditConsolidationReport with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));
        
        try
        {
            var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update InternalAuditConsolidationReport with ID {Id}", internalAuditConsolidationReportModel.Id);
            throw new InvalidOperationException($"Failed to update InternalAuditConsolidationReport with ID '{internalAuditConsolidationReportModel.Id}'", ex);
        }
    }
}
