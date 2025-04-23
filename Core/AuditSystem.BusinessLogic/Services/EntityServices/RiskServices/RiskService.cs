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

internal sealed class RiskService(
    IRepository<Guid, Risk> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskService> logger) : 
    BaseEntityCacheService<Risk>(
        cacheService,
        logger,
        CacheKeys.Risk,
        RiskTags,
        ListTags
    ),
    IRiskService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for RiskModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Risk");
            throw new InvalidOperationException("Failed to create Risk.", ex);
        }
    }

    public async Task DeleteRiskAsync(Guid riskId)
    {
        var cacheKey = GetCacheKey(riskId);
        
        try
        {
            var entity = await repository.GetByIdAsync(riskId)
                ?? throw new KeyNotFoundException($"Risk with ID {riskId} not found.");

            await repository.DeleteAsync(riskId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Risk with ID {RiskId} not found", riskId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Risk ID {RiskId}", riskId);
            throw new InvalidOperationException($"Failed to delete Risk: {riskId}", ex);
        }
    }

    public async Task<RiskModel> GetRiskByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<Risk>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<RiskModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Risk with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<RiskModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Risk with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Risk with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<RiskModel>> Search(string? Name, Page page)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResult<RiskModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateRiskAsync(RiskModel riskModel)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));
        
        try
        {
            var entity = mapper.Map<Risk>(riskModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Risk with ID {Id}", riskModel.Id);
            throw new InvalidOperationException($"Failed to update Risk with ID '{riskModel.Id}'", ex);
        }
    }
}