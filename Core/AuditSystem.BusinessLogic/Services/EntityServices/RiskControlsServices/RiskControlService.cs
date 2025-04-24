using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlService(
    IRepository<Guid, RiskControls> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskControlService> logger) : 
    UnifiedCacheService<Guid, RiskControls, RiskControlsModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.RiskControl,
        RiskControlTags,
        ListTags
    ),
    IRiskControlService
{
    private static readonly string[] RiskControlTags = ["risk-controls", "risk-control-list"];
    private static readonly string[] ListTags = ["risk-control-list"];

    public async Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlModel, nameof(riskControlModel));

        var entity = mapper.Map<RiskControls>(riskControlModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);
        await cacheService.RemoveCacheByTagAsync(ListTags);

        return createdEntity.Id;
    }

    public async Task DeleteRiskControlAsync(Guid riskControlId)
    {
        var cacheKey = GetCacheKey(riskControlId);
       
        var entity = await repository.GetByIdAsync(riskControlId)
            ?? throw new KeyNotFoundException($"RiskControl with ID {riskControlId} not found.");

        await repository.DeleteAsync(riskControlId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<RiskControlsModel> GetRiskControlByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<RiskControls>(cacheKey);
        if (entity != null)
            return mapper.Map<RiskControlsModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"RiskControl with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskControlsModel>(entity);
    }

    public Task<PaginationResult<RiskControlsModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlModel, nameof(riskControlModel));

        var entity = mapper.Map<RiskControls>(riskControlModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
