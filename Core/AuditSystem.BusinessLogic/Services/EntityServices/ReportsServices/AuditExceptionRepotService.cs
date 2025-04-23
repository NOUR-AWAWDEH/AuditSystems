using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class AuditExceptionRepotService(
    IRepository<Guid, AuditExceptionReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditExceptionRepotService> logger) : 
    BaseEntityCacheService<AuditExceptionReport>(
        cacheService,
        logger,
        CacheKeys.AuditExceptionReport,
        AuditExceptionReportTags,
        ListTags
    ),
    IAuditExceptionReportService
{
    private static readonly string[] AuditExceptionReportTags = ["audit-exception-reports", "audit-exception-report-list"];
    private static readonly string[] ListTags = ["audit-exception-report-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));

        try
        {
            var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditExceptionReportModel");
            throw new ArgumentException("Mapping failed for AuditExceptionReportModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating the AuditExceptionReport");
            throw new InvalidOperationException("An error occurred while creating the AuditExceptionReport", ex);
        }
    }
    
    public async Task DeleteAuditExceptionReportAsync(Guid auditExceptionReportId)
    {
        var cacheKey = GetCacheKey(auditExceptionReportId);
        
        try
        {
            var entity = await repository.GetByIdAsync(auditExceptionReportId)
                ?? throw new KeyNotFoundException($"AuditExceptionReport with ID {auditExceptionReportId} not found.");

            await repository.DeleteAsync(auditExceptionReportId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "AuditExceptionReport with ID {ReportId} not found", auditExceptionReportId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditExceptionReport ID {ReportId}", auditExceptionReportId);
            throw new InvalidOperationException($"Failed to delete AuditExceptionReport: {auditExceptionReportId}", ex);
        }
    }
    
    public async Task<AuditExceptionReportModel> GetAuditExceptionReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditExceptionReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditExceptionReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"AuditExceptionReport with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<AuditExceptionReportModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditExceptionReport with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditExceptionReport with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<AuditExceptionReportModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));

        try
        {
            var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditExceptionReport with ID {Id}", auditExceptionReportModel.Id);
            throw new InvalidOperationException($"Failed to update AuditExceptionReport with ID '{auditExceptionReportModel.Id}'", ex);
        }
    }
}
