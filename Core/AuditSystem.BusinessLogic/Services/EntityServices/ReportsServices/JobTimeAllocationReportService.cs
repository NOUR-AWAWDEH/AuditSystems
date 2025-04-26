using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
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
    UnifiedCacheService<Guid, JobTimeAllocationReport, JobTimeAllocationReportModel>(
        repository,
        mapper,
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
       
        var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteJobTimeAllocationReportAsync(Guid jobTimeAllocationReportId)
    {
        var cacheKey = GetCacheKey(jobTimeAllocationReportId);
        
        var entity = await repository.GetByIdAsync(jobTimeAllocationReportId)
            ?? throw new KeyNotFoundException($"JobTimeAllocationReport with ID {jobTimeAllocationReportId} not found.");

        await repository.DeleteAsync(jobTimeAllocationReportId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<JobTimeAllocationReportModel> GetJobTimeAllocationReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<JobTimeAllocationReport>(cacheKey);
        if (entity != null)
            return mapper.Map<JobTimeAllocationReportModel>(entity);
        
        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"JobTimeAllocationReport with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<JobTimeAllocationReportModel>(entity);
    }

    public async Task<PaginationResult<JobTimeAllocationReportModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<JobTimeAllocationReportModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<JobTimeAllocationReportModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        ArgumentNullException.ThrowIfNull(jobTimeAllocationReportModel, nameof(jobTimeAllocationReportModel));
        
        var entity = mapper.Map<JobTimeAllocationReport>(jobTimeAllocationReportModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
