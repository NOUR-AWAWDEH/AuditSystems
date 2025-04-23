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

internal sealed class ReportingFollowUpService(
    IRepository<Guid, ReportingFollowUp> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ReportingFollowUpService> logger) : 
    BaseEntityCacheService<ReportingFollowUp>(
        cacheService,
        logger,
        CacheKeys.ReportingFollowUp,
        ReportingFollowUpTags,
        ListTags
    ),
    IReportingFollowUpService
{
    private static readonly string[] ReportingFollowUpTags = ["reporting-follow-ups", "reporting-follow-up-list"];
    private static readonly string[] ListTags = ["reporting-follow-up-list"];

    public async Task<Guid> CreateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        ArgumentNullException.ThrowIfNull(reportingFollowUpModel, nameof(reportingFollowUpModel));

        try
        {
            var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for ReportingFollowUpModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create ReportingFollowUp");
            throw new InvalidOperationException("Failed to create ReportingFollowUp.", ex);
        }
    }
    
    public async Task DeleteReportingFollowUpAsync(Guid reportingFollowUpId)
    {
        var cacheKey = GetCacheKey(reportingFollowUpId);
        
        try
        {
            var entity = await repository.GetByIdAsync(reportingFollowUpId)
                ?? throw new KeyNotFoundException($"ReportingFollowUp with ID {reportingFollowUpId} not found.");

            await repository.DeleteAsync(reportingFollowUpId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "ReportingFollowUp with ID {ReportId} not found", reportingFollowUpId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete ReportingFollowUp ID {ReportId}", reportingFollowUpId);
            throw new InvalidOperationException($"Failed to delete ReportingFollowUp: {reportingFollowUpId}", ex);
        }
    }
    
        public async Task<ReportingFollowUpModel> GetReportingFollowUpByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<ReportingFollowUp>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<ReportingFollowUpModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"ReportingFollowUp with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<ReportingFollowUpModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve ReportingFollowUp with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve ReportingFollowUp with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<ReportingFollowUpModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        ArgumentNullException.ThrowIfNull(reportingFollowUpModel, nameof(reportingFollowUpModel));

        try
        {
            var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update ReportingFollowUp with ID {Id}", reportingFollowUpModel.Id);
            throw new InvalidOperationException($"Failed to update ReportingFollowUp with ID '{reportingFollowUpModel.Id}'", ex);
        }
    }
}