using AuditSystem.Application.Constants;
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
    BaseEntityCacheService<RiskAssessment>(
        cacheService,
        logger,
        CacheKeys.RiskAssessment,
        RiskAssessmentTags,
        ListTags
    ),
    IRiskAssessmentService
{
    private static readonly string[] RiskAssessmentTags = ["risk-assessments", "risk-assessment-list"];
    private static readonly string[] ListTags = ["risk-assessment-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));

        try
        {
            var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RiskAssessmentModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create RiskAssessment");
            throw new InvalidOperationException("Failed to create RiskAssessment.", ex);
        }
    }

    public async Task DeleteRiskAssessmentAsync(Guid riskAssessmentId)
    {
        var cacheKey = GetCacheKey(riskAssessmentId);
        
        try
        {
            var entity = await repository.GetByIdAsync(riskAssessmentId)
                ?? throw new KeyNotFoundException($"RiskAssessment with ID {riskAssessmentId} not found.");

            await repository.DeleteAsync(riskAssessmentId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "RiskAssessment with ID {AssessmentId} not found", riskAssessmentId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete RiskAssessment ID {AssessmentId}", riskAssessmentId);
            throw new InvalidOperationException($"Failed to delete RiskAssessment: {riskAssessmentId}", ex);
        }
    }

    public async Task<RiskAssessmentModel> GetRiskAssessmentByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<RiskAssessment>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<RiskAssessmentModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"RiskAssessment with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<RiskAssessmentModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve RiskAssessment with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve RiskAssessment with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<RiskAssessmentModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));

        try
        {
            var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update RiskAssessment with ID {Id}", riskAssessmentModel.Id);
            throw new InvalidOperationException($"Failed to update RiskAssessment with ID '{riskAssessmentModel.Id}'", ex);
        }
    }
}
