using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskService(
    IRepository<Guid, Risk> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskService> logger) : 
    UnifiedCacheService<Guid, Risk, RiskModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Risk,
        RiskTags,
        ListTags
    ),
    IRiskService
{
    private static readonly string[] RiskTags = ["risks", "risk-list"];
    private static readonly string[] ListTags = ["risk-list"];

    public async Task<Guid> CreateRiskAsync(RiskModel riskModel)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));

        var entity = mapper.Map<Risk>(riskModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteRiskAsync(Guid riskId)
    {
        var cacheKey = GetCacheKey(riskId);

        var entity = await repository.GetByIdAsync(riskId)
            ?? throw new KeyNotFoundException($"Risk with ID {riskId} not found.");

        await repository.DeleteAsync(riskId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<RiskModel> GetRiskByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<Risk>(cacheKey);
        if (entity != null)
            return mapper.Map<RiskModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Risk with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskModel>(entity);
    }

    public async Task<PaginationResult<RiskModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RiskModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<RiskModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateRiskAsync(RiskModel riskModel)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));
        
        var entity = mapper.Map<Risk>(riskModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}