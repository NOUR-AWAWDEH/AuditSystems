using AutoMapper;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Application.Constants;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskService(
    IRepository<Guid, Risk> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskService
{
    private static readonly string[] RiskTags = ["risks", "risk-list"];
    private static readonly string[] ListTags = ["risk-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskAsync(RiskModel riskModel)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));

        try
        {
            var entity = mapper.Map<Risk>(riskModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.Risk, createdEntity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create risk.", ex);
        }
    }
}