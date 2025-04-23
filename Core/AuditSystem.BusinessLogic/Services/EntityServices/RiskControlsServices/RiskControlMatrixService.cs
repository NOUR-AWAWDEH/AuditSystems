using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlMatrixService(
    IRepository<Guid, RiskControlMatrix> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskControlMatrixService> logger) : 
    BaseEntityCacheService<RiskControlMatrix>(
        cacheService,
        logger,
        CacheKeys.RiskControlMatrix,
        RiskControlMatrixTags,
        ListTags
    ),
    IRiskControlMatrixService
{
    private static readonly string[] RiskControlMatrixTags = ["risk-control-matrices", "risk-control-matrix-list"];
    private static readonly string[] ListTags = ["risk-control-matrix-list"];

    public async Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));

        try
        {
            var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RiskControlMatrixModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create RiskControlMatrix");
            throw new InvalidOperationException("Failed to create RiskControlMatrix.", ex);
        }
    }
    
    public async Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId)
    {
        var cacheKey = GetCacheKey(riskControlMatrixId);
        
        try
        {
            var entity = await repository.GetByIdAsync(riskControlMatrixId)
                ?? throw new KeyNotFoundException($"RiskControlMatrix with ID {riskControlMatrixId} not found.");

            await repository.DeleteAsync(riskControlMatrixId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "RiskControlMatrix with ID {MatrixId} not found", riskControlMatrixId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete RiskControlMatrix ID {MatrixId}", riskControlMatrixId);
            throw new InvalidOperationException($"Failed to delete RiskControlMatrix: {riskControlMatrixId}", ex);
        }
    }
    
    public async Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<RiskControlMatrix>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<RiskControlMatrixModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"RiskControlMatrix with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<RiskControlMatrixModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve RiskControlMatrix with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve RiskControlMatrix with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));

        try
        {
            var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update RiskControlMatrix with ID {Id}", riskControlMatrixModel.Id);
            throw new InvalidOperationException($"Failed to update RiskControlMatrix with ID '{riskControlMatrixModel.Id}'", ex);
        }
    }
}
