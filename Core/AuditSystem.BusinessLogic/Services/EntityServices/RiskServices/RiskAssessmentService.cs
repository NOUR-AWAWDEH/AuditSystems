using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskAssessmentService(
    IRepository<Guid, RiskAssessment> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskAssessmentService
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

            var cacheKey = string.Format(CacheKeys.RiskAssessment, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskAssessmentTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create RiskAssessment.", ex);
        }
    }

    public async Task DeleteRiskAssessmentAsync(Guid riskAssessmentId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(riskAssessmentId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskAssessment with ID {riskAssessmentId} not found.");
            }

            await repository.DeleteAsync(riskAssessmentId);

            var cacheKey = string.Format(CacheKeys.RiskAssessment, riskAssessmentId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete RiskAssessment.", ex);
        }
    }

    public async Task<RiskAssessmentModel> GetRiskAssessmentByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.RiskAssessment, id);

            var cachedRiskAssessment = await cacheService.GetAsync<RiskAssessment>(cacheKey);
            if (cachedRiskAssessment != null)
            {
                return mapper.Map<RiskAssessmentModel>(cachedRiskAssessment);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskAssessment with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskAssessmentTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<RiskAssessmentModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get RiskAssessment ID {id}.", ex);
        }
    }

    public async Task UpdateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));

        try
        {
            var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskAssessment, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskAssessmentTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update RiskAssessment.", ex);
        }
    }
}
