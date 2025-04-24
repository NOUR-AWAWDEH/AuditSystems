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

internal sealed class RiskFactorService(
    IRepository<Guid, RiskFactor> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskFactorService> logger) : 
    UnifiedCacheService<Guid, RiskFactor, RiskFactorModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.RiskFactor,
        RiskFactorTags,
        ListTags
    ),
    IRiskFactorService
{
    private static readonly string[] RiskFactorTags = ["risk-factors", "risk-factor-list"];
    private static readonly string[] ListTags = ["risk-factor-list"];

    public async Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));
       
        var entity = mapper.Map<RiskFactor>(riskFactorModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteRiskFactorAsync(Guid riskFactorId)
    {
        var cacheKey = GetCacheKey(riskFactorId);
        
        var entity = await repository.GetByIdAsync(riskFactorId)
            ?? throw new KeyNotFoundException($"RiskFactor with ID {riskFactorId} not found.");

        await repository.DeleteAsync(riskFactorId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<RiskFactorModel> GetRiskFactorByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
       
        var entity = await cacheService.GetAsync<RiskFactor>(cacheKey);
        if (entity != null)
            return mapper.Map<RiskFactorModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"RiskFactor with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskFactorModel>(entity);
    }

    public Task<PaginationResult<RiskFactorModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));
        
        var entity = mapper.Map<RiskFactor>(riskFactorModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}