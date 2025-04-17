using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class BusinessObjectiveService(
    IRepository<Guid, BusinessObjective> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IBusinessObjectiveService
{
    private static readonly string[] BusinessObjectiveTags = ["business-objectives", "business-objective-list"];
    private static readonly string[] ListTags = ["business-objective-list"]; // Tags for collections only

    public async Task<Guid> CreateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(businessObjectiveModel, nameof(businessObjectiveModel));

        try
        {
            var entity = mapper.Map<BusinessObjective>(businessObjectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.BusinessObjective, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: BusinessObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create BusinessObjective.", ex);
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

    public Task<BusinessObjectiveModel> GetBusinessObjectiveByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
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
