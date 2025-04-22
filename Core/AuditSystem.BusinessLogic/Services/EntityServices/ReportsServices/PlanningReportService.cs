using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class PlanningReportService(
    IRepository<Guid, PlanningReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<PlanningReportService> logger) : 
    BaseEntityCacheService<PlanningReport>(
        cacheService,
        logger,
        CacheKeys.PlanningReport,
        PlanningReportTags,
        ListTags
    ),
    IPlanningReportService
{
    private static readonly string[] PlanningReportTags = ["planning-reports", "planning-report-list"];
    private static readonly string[] ListTags = ["planning-report-list"];

    public async Task<Guid> CreatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));

        try
        {
            var entity = mapper.Map<PlanningReport>(planningReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for PlanningReportModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create PlanningReport");
            throw new InvalidOperationException("Failed to create PlanningReport.", ex);
        }
    }
    
    public async Task DeletePlanningReportAsync(Guid planningReportId)
    {
        var cacheKey = GetCacheKey(planningReportId);
        
        try
        {
            var entity = await repository.GetByIdAsync(planningReportId)
                ?? throw new KeyNotFoundException($"PlanningReport with ID {planningReportId} not found.");

            await repository.DeleteAsync(planningReportId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "PlanningReport with ID {ReportId} not found", planningReportId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete PlanningReport ID {ReportId}", planningReportId);
            throw new InvalidOperationException($"Failed to delete PlanningReport: {planningReportId}", ex);
        }
    }
    
    public async Task<PlanningReportModel> GetPlanningReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<PlanningReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<PlanningReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"PlanningReport with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<PlanningReportModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve PlanningReport with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve PlanningReport with ID '{id}'", ex);
        }
    }
    
    public async Task UpdatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));
        
        try
        {
            var entity = mapper.Map<PlanningReport>(planningReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update PlanningReport with ID {Id}", planningReportModel.Id);
            throw new InvalidOperationException($"Failed to update PlanningReport with ID '{planningReportModel.Id}'", ex);
        }
    }
}
