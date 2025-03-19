using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class SpecificRiskFactorService(
    IRepository<Guid, SpecificRiskFactor> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISpecificRiskFactorService
{
    private static readonly string[] SpecificRiskFactorTags = ["specific-risk-factors", "specific-risk-factor-list"];
    private static readonly string[] ListTags = ["specific-risk-factor-list"]; // Tags for collections only

    public async Task<Guid> CreateSpecificRiskFactorAsync(SpecificRiskFactorModel specificRiskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(specificRiskFactorModel, nameof(specificRiskFactorModel));
        
        try
        {
            var entity = mapper.Map<SpecificRiskFactor>(specificRiskFactorModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.SpecificRiskFactor, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SpecificRiskFactorTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SpecificRiskFactor.", ex);
        }
    }
}