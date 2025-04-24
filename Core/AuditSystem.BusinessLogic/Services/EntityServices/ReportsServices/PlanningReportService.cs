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

internal sealed class PlanningReportService(
    IRepository<Guid, PlanningReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<PlanningReportService> logger) : 
    UnifiedCacheService<Guid, PlanningReport, PlanningReportModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<PlanningReport>(planningReportModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeletePlanningReportAsync(Guid planningReportId)
    {
        var cacheKey = GetCacheKey(planningReportId);
        
        var entity = await repository.GetByIdAsync(planningReportId)
            ?? throw new KeyNotFoundException($"PlanningReport with ID {planningReportId} not found.");

        await repository.DeleteAsync(planningReportId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<PlanningReportModel> GetPlanningReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<PlanningReport>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<PlanningReportModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"PlanningReport with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<PlanningReportModel>(entity);
    }

    public Task<PaginationResult<PlanningReportModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));
        
        var entity = mapper.Map<PlanningReport>(planningReportModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
