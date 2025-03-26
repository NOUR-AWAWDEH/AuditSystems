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

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.Objective, createdEntity.Id);

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
}
