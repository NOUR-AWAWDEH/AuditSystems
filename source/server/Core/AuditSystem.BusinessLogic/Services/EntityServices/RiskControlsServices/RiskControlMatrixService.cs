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

internal sealed class RiskControlMatrixService(
    IRepository<Guid, RiskControlMatrix> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskControlMatrixService> logger) : 
    UnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.RiskControlMatrix,
        RiskControlMatrixTags,
        ListTags
    ),
    IRiskControlMatrixService
{
    private static readonly string[] RiskControlMatrixTags = ["risk-control-matrices", "risk-control-matrix-list"];
    private static readonly string[] ListTags = ["risk-control-matrix-list"];

    public async Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));
       
        var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId)
    {
        var cacheKey = GetCacheKey(riskControlMatrixId);
        
        var entity = await repository.GetByIdAsync(riskControlMatrixId)
            ?? throw new KeyNotFoundException($"RiskControlMatrix with ID {riskControlMatrixId} not found.");

        await repository.DeleteAsync(riskControlMatrixId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<RiskControlMatrix>(cacheKey);
        if (entity != null)
            return mapper.Map<RiskControlMatrixModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"RiskControlMatrix with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskControlMatrixModel>(entity);
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));

        var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
