using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlMatrixService(
    IRepository<Guid, RiskControlMatrix> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskControlMatrixService
{
    private static readonly string[] RiskControlMatrixTags = ["risk-control-matrices", "risk-control-matrix-list"];
    private static readonly string[] ListTags = ["risk-control-matrix-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));

        try
        {
            var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskControlMatrix, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskControlMatrixTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create RiskControlMatrix.", ex);
        }
    }
    public async Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(riskControlMatrixId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskControlMatrix with ID {riskControlMatrixId} not found.");
            }

            await repository.DeleteAsync(riskControlMatrixId);

            var cacheKey = string.Format(CacheKeys.RiskControlMatrix, riskControlMatrixId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete RiskControlMatrix.", ex);
        }
    }
    public async Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.RiskControlMatrix, id);

            var cachedEntity = await cacheService.GetAsync<RiskControlMatrix>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<RiskControlMatrixModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskControlMatrix with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskControlMatrixTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<RiskControlMatrixModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get RiskControlMatrix by ID {id}.", ex);
        }
    }
    public async Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));

        try
        {
            var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskControlMatrix, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskControlMatrixTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update RiskControlMatrix.", ex);
        }
    }
}
