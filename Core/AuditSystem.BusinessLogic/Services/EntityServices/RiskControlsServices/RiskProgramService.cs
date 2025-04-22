using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
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
    BaseEntityCacheService<RiskProgram>(
        cacheService,
        logger,
        CacheKeys.RiskProgram,
        RiskProgramTags,
        ListTags
    ),
    IRiskProgramService
{
    private static readonly string[] RiskProgramTags = ["risk-programs", "risk-program-list"];
    private static readonly string[] ListTags = ["risk-program-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        try
        {
            var entity = mapper.Map<RiskProgram>(riskProgramModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RiskProgramModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create RiskProgram");
            throw new InvalidOperationException("Failed to create RiskProgram.", ex);
        }
    }

    public async Task DeleteRiskProgramAsync(Guid riskProgramId)
    {
        var cacheKey = GetCacheKey(riskProgramId);
        
        try
        {
            var entity = await repository.GetByIdAsync(riskProgramId)
                ?? throw new KeyNotFoundException($"RiskProgram with ID {riskProgramId} not found.");

            await repository.DeleteAsync(riskProgramId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "RiskProgram with ID {RiskProgramId} not found", riskProgramId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete RiskProgram ID {RiskProgramId}", riskProgramId);
            throw new InvalidOperationException($"Failed to delete RiskProgram: {riskProgramId}", ex);
        }
    }

    public async Task<RiskProgramModel> GetRiskProgramByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve RiskProgram with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve RiskProgram with ID '{id}'", ex);
        }
    }

    public async Task UpdateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        try
        {
            var entity = mapper.Map<RiskProgram>(riskProgramModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update RiskProgram with ID {Id}", riskProgramModel.Id);
            throw new InvalidOperationException($"Failed to update RiskProgram with ID '{riskProgramModel.Id}'", ex);
        }
    }
}
