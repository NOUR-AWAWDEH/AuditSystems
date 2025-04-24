using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal class ObjectiveService(
    IRepository<Guid, Objective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ObjectiveService> logger) :
    UnifiedCacheService<Guid, Objective, ObjectiveModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Objective,
        ObjectiveTags,
        ListTags
    ),
    IObjectiveService
{
    private static readonly string[] ObjectiveTags = ["objectives", "objective-list"];
    private static readonly string[] ListTags = ["objective-list"];
    
    public async Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(ObjectiveModel));

        var entity = mapper.Map<Objective>(objectiveModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteObjectiveAsync(Guid objectiveId)
    {
        var cacheKey = GetCacheKey(objectiveId);
       
        var entity = await repository.GetByIdAsync(objectiveId)
                ?? throw new KeyNotFoundException($"Objective with ID {objectiveId} not found.");
        
        await repository.DeleteAsync(objectiveId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<ObjectiveModel> GetObjectiveByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<Objective>(cacheKey);
        if (cachedEntity != null)
        {
            return mapper.Map<ObjectiveModel>(cachedEntity);
        }

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Objective with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<ObjectiveModel>(entity);
    }

    public Task<PaginationResult<ObjectiveModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateObjectiveAsync(ObjectiveModel objectiveModel)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(objectiveModel));
        
        var entity = mapper.Map<Objective>(objectiveModel);
        await repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
