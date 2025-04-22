using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
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
    BaseEntityCacheService<RiskFactor>(
        cacheService,
        logger,
        CacheKeys.RiskFactor,
        RiskFactorTags,
        ListTags
    ),
    IRiskFactorService
{
    private static readonly string[] RiskFactorTags = ["risk-factors", "risk-factor-list"];
    private static readonly string[] ListTags = ["risk-factor-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));

        try
        {
            var entity = mapper.Map<RiskFactor>(riskFactorModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RiskFactorModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create RiskFactor");
            throw new InvalidOperationException("Failed to create RiskFactor.", ex);
        }
    }

    public async Task DeleteRiskFactorAsync(Guid riskFactorId)
    {
        var cacheKey = GetCacheKey(riskFactorId);
        
        try
        {
            var entity = await repository.GetByIdAsync(riskFactorId)
                ?? throw new KeyNotFoundException($"RiskFactor with ID {riskFactorId} not found.");

            await repository.DeleteAsync(riskFactorId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "RiskFactor with ID {RiskFactorId} not found", riskFactorId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete RiskFactor ID {RiskFactorId}", riskFactorId);
            throw new InvalidOperationException($"Failed to delete RiskFactor: {riskFactorId}", ex);
        }
    }

    public async Task<RiskFactorModel> GetRiskFactorByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<RiskFactor>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<RiskFactorModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"RiskFactor with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<RiskFactorModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve RiskFactor with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve RiskFactor with ID '{id}'", ex);
        }
    }

    public async Task UpdateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));
        
        try
        {
            var entity = mapper.Map<RiskFactor>(riskFactorModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update RiskFactor with ID {Id}", riskFactorModel.Id);
            throw new InvalidOperationException($"Failed to update RiskFactor with ID '{riskFactorModel.Id}'", ex);
        }
    }
}