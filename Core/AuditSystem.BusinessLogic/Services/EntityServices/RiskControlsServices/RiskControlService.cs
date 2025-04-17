using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlService(
    IRepository<Guid, RiskControls> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskControlService
{
    private static readonly string[] RiskControlTags = ["risk-controls", "risk-control-list"];
    private static readonly string[] ListTags = ["risk-control-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlModel, nameof(riskControlModel));

        try
        {
            var entity = mapper.Map<RiskControls>(riskControlModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskControl, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskControlTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create RiskControl.", ex);
        }
    }

    public async Task DeleteRiskControlAsync(Guid riskControlId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(riskControlId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskControl with ID {riskControlId} not found.");
            }

            await repository.DeleteAsync(riskControlId);

            var cacheKey = string.Format(CacheKeys.RiskControl, riskControlId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete RiskControl.", ex);
        }
    }

    public Task<RiskControlsModel> GetRiskControlByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlModel, nameof(riskControlModel));

        try
        {
            var entity = mapper.Map<RiskControls>(riskControlModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskControl, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskControlTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update RiskControl.", ex);
        }
    }
}
