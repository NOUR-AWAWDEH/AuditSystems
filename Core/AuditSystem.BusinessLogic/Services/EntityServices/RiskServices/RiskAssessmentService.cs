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

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.RiskAssessment, createdEntity.Id);

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
}
