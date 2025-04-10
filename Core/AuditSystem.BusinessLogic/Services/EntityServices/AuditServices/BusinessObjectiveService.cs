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
}
