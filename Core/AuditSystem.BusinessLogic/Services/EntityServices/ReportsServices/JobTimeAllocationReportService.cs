using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class JobTimeAllocationReportService(
    IRepository<Guid, JobTimeAllocationReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobTimeAllocationReportService> logger) : 
    BaseEntityCacheService<JobTimeAllocationReport>(
        cacheService,
        logger,
        CacheKeys.JobTimeAllocationReport,
        JobTimeAllocationReportTags,
        ListTags
    ),
    IJobTimeAllocationReportService
{
    private static readonly string[] JobTimeAllocationReportTags = ["job-time-allocation-reports", "job-time-allocation-report-list"];
    private static readonly string[] ListTags = ["job-time-allocation-report-list"];

    public async Task<Guid> CreateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        ArgumentNullException.ThrowIfNull(jobTimeAllocationReportModel, nameof(jobTimeAllocationReportModel));

        try
        {
            var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for JobTimeAllocationReportModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create JobTimeAllocationReport");
            throw new InvalidOperationException("Failed to create JobTimeAllocationReport.", ex);
        }
    }
    
    public async Task DeleteJobTimeAllocationReportAsync(Guid jobTimeAllocationReportId)
    {
        var cacheKey = GetCacheKey(jobTimeAllocationReportId);
        
        try
        {
            var entity = await repository.GetByIdAsync(jobTimeAllocationReportId)
                ?? throw new KeyNotFoundException($"JobTimeAllocationReport with ID {jobTimeAllocationReportId} not found.");

            await repository.DeleteAsync(jobTimeAllocationReportId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "JobTimeAllocationReport with ID {ReportId} not found", jobTimeAllocationReportId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete JobTimeAllocationReport ID {ReportId}", jobTimeAllocationReportId);
            throw new InvalidOperationException($"Failed to delete JobTimeAllocationReport: {jobTimeAllocationReportId}", ex);
        }
    }
    
    public async Task<JobTimeAllocationReportModel> GetJobTimeAllocationReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<JobTimeAllocationReport>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<JobTimeAllocationReportModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"JobTimeAllocationReport with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<JobTimeAllocationReportModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve JobTimeAllocationReport with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve JobTimeAllocationReport with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        ArgumentNullException.ThrowIfNull(jobTimeAllocationReportModel, nameof(jobTimeAllocationReportModel));
        
        try
        {
            var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update JobTimeAllocationReport with ID {Id}", jobTimeAllocationReportModel.Id);
            throw new InvalidOperationException($"Failed to update JobTimeAllocationReport with ID '{jobTimeAllocationReportModel.Id}'", ex);
        }
    }
}
