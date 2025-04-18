using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal class ObjectiveService(
    IRepository<Guid, Objective> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IObjectiveService
{
    private static readonly string[] ObjectiveTags = ["objectives", "objective-list"];
    private static readonly string[] ListTags = ["objective-list"];
    public async Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(ObjectiveModel));

        try
        {
            var entity = mapper.Map<Objective>(objectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Objective, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Objective.", ex);
        }
    }
    public async Task DeleteObjectiveAsync(Guid objectiveId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(objectiveId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Objective with ID {objectiveId} not found.");
            }
            
            await repository.DeleteAsync(objectiveId);
            
            var cacheKey = string.Format(CacheKeys.Objective, entity.Id);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Objective.", ex);
        }
    }
    public async Task<ObjectiveModel> GetObjectiveByIdAsync(Guid Id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.Objective, Id);

            var cachedObjectiveModel = await cacheService.GetAsync<Objective>(cacheKey);
            if (cachedObjectiveModel != null)
            {
                return mapper.Map<ObjectiveModel>(cachedObjectiveModel);
            }

            var entity = await repository.GetByIdAsync(Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Objective with ID {Id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ObjectiveTags,
                expiration: CacheExpirations.MediumTerm  
            );

            return mapper.Map<ObjectiveModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to get Objective.", ex);
        }
    }
    public async Task UpdateObjectiveAsync(ObjectiveModel objectiveModel)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(objectiveModel));
        
        try 
        {
            var entity = mapper.Map<Objective>(objectiveModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Objective, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Objective.", ex);
        }
    }
}
