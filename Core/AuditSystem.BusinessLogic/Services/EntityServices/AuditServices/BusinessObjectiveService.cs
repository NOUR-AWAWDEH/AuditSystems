using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class BusinessObjectiveService(
    IRepository<Guid, BusinessObjective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<BusinessObjectiveService> logger) : 
    BaseEntityCacheService<BusinessObjective>(
        cacheService,
        logger,
        CacheKeys.BusinessObjective,
        BusinessObjectiveTags,
        ListTags
    ),
    IBusinessObjectiveService
{
    private static readonly string[] BusinessObjectiveTags = ["business-objectives", "business-objective-list"];
    private static readonly string[] ListTags = ["business-objective-list"];
    
    public async Task<Guid> CreateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));

        try
        {
            var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey,entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "AutoMapper mapping exception occurred while creating BusinessObjectiveModel.");
            throw new ArgumentException("AutoMapper mapping exception occurred while creating BusinessObjectiveModel.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating BusinessObjectiveModel.");
            throw new InvalidOperationException("An error occurred while creating BusinessObjectiveModel.", ex);
        }
    }
    
    public async Task DeleteBusinessObjectiveAsync(Guid businessObjectiveId)
    {
        var cacheKey = GetCacheKey(businessObjectiveId);
        try 
        {
            var entity = await repository.GetByIdAsync(businessObjectiveId)
                    ?? throw new KeyNotFoundException($"BusinessObjective with ID {businessObjectiveId} not found.");
            
            
            await repository.DeleteAsync(businessObjectiveId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex,"BusinessObjective with ID : {id} not found.",businessObjectiveId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete BusinessObjective.");
            throw new InvalidOperationException("Failed to delete BusinessObjective.", ex);
        }
    }
    
    public async Task<BusinessObjectiveModel> GetBusinessObjectiveByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<BusinessObjective>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<BusinessObjectiveModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"BusinessObjective with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<BusinessObjectiveModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve BusinessObjective with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve BusinessObjective with ID '{id}'", ex);
        }
    }

    public async Task UpdateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));
        
        try 
        {
            var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update BusinessObjective with ID {Id}", businessObjectiveModel.Id);
            throw new InvalidOperationException($"Failed to update BusinessObjective with ID '{businessObjectiveModel.Id}'", ex);
        }
    }
}
