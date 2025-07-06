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

internal sealed class RiskProgramService(
    IRepository<Guid, RiskProgram> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskProgramService> logger) : 
    UnifiedCacheService<Guid, RiskProgram, RiskProgramModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.RiskProgram,
        RiskProgramTags,
        ListTags
    ),
    IRiskProgramService
{
    private static readonly string[] RiskProgramTags = ["risk-programs", "risk-program-list"];
    private static readonly string[] ListTags = ["risk-program-list"];

    public async Task<Guid> CreateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        var entity = mapper.Map<RiskProgram>(riskProgramModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteRiskProgramAsync(Guid riskProgramId)
    {
        var cacheKey = GetCacheKey(riskProgramId);
        
        var entity = await repository.GetByIdAsync(riskProgramId)
            ?? throw new KeyNotFoundException($"RiskProgram with ID {riskProgramId} not found.");

        await repository.DeleteAsync(riskProgramId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<RiskProgramModel> GetRiskProgramByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<RiskProgram>(cacheKey);
        if (entity != null)
        {
            return mapper.Map<RiskProgramModel>(entity);
        }

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"RiskProgram with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskProgramModel>(entity);
    }

    public async Task<PaginationResult<RiskProgramModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RiskProgramModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<RiskProgramModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        var entity = mapper.Map<RiskProgram>(riskProgramModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
