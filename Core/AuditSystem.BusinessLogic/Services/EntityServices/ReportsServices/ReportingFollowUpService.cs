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

internal sealed class ReportingFollowUpService(
    IRepository<Guid, ReportingFollowUp> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ReportingFollowUpService> logger) : 
    UnifiedCacheService<Guid, ReportingFollowUp, ReportingFollowUpModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteReportingFollowUpAsync(Guid reportingFollowUpId)
    {
        var cacheKey = GetCacheKey(reportingFollowUpId);
       
        var entity = await repository.GetByIdAsync(reportingFollowUpId)
            ?? throw new KeyNotFoundException($"ReportingFollowUp with ID {reportingFollowUpId} not found.");

        await repository.DeleteAsync(reportingFollowUpId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<ReportingFollowUpModel> GetReportingFollowUpByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<ReportingFollowUp>(cacheKey);
        if (entity != null)
            return mapper.Map<ReportingFollowUpModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"ReportingFollowUp with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<ReportingFollowUpModel>(entity); 
    }

    public async Task<PaginationResult<ReportingFollowUpModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<ReportingFollowUpModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<ReportingFollowUpModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        ArgumentNullException.ThrowIfNull(reportingFollowUpModel, nameof(reportingFollowUpModel));

        var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}