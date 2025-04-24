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

internal sealed class RiskAssessmentService(
    IRepository<Guid, RiskAssessment> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskAssessmentService> logger) : 
    UnifiedCacheService<Guid, RiskAssessment, RiskAssessmentModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.RiskAssessment,
        RiskAssessmentTags,
        ListTags
    ),
    IRiskAssessmentService
{
    private static readonly string[] RiskAssessmentTags = ["risk-assessments", "risk-assessment-list"];
    private static readonly string[] ListTags = ["risk-assessment-list"];

    public async Task<Guid> CreateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));
        
        var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteRiskAssessmentAsync(Guid riskAssessmentId)
    {
        var cacheKey = GetCacheKey(riskAssessmentId);
        
        var entity = await repository.GetByIdAsync(riskAssessmentId)
            ?? throw new KeyNotFoundException($"RiskAssessment with ID {riskAssessmentId} not found.");

        await repository.DeleteAsync(riskAssessmentId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<RiskAssessmentModel> GetRiskAssessmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<RiskAssessment>(cacheKey);
        if (entity != null)
            return mapper.Map<RiskAssessmentModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"RiskAssessment with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<RiskAssessmentModel>(entity);
    }

    public Task<PaginationResult<RiskAssessmentModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));

        var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
