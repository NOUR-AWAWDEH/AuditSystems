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
        try 
        {
            var entity = await repository.GetByIdAsync(businessObjectiveId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"BusinessObjective with ID {businessObjectiveId} not found.");
            }
            
            await repository.DeleteAsync(businessObjectiveId);
            
            var cacheKey = string.Format(CacheKeys.BusinessObjective, businessObjectiveId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete BusinessObjective.", ex);
        }
    }
    
    public async Task<BusinessObjectiveModel> GetBusinessObjectiveByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.BusinessObjective, id);

            var cachedBusinessObjectiveModel = await cacheService.GetAsync<BusinessObjective>(cacheKey);
            if (cachedBusinessObjectiveModel!= null)
            {
                return mapper.Map<BusinessObjectiveModel>(cachedBusinessObjectiveModel);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"BusinessObjective with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: BusinessObjectiveTags,
                expiration: CacheExpirations.MediumTerm 
            );

            return mapper.Map<BusinessObjectiveModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get BusinessObjective ID {id}", ex);
        }
    }

    public async Task UpdateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));
        
        try 
        {
            var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.BusinessObjective, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: BusinessObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update BusinessObjective.", ex);
        }
    }
}
