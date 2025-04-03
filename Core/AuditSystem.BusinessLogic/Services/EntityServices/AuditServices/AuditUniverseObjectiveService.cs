using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseObjectiveService(
    IRepository<Guid, AuditUniverseObjective> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseObjectiveService
{
    private static readonly string[] AuditUniverseObjectiveTags = ["audit-universe-objectives", "audit-universe-objective-list"];
    private static readonly string[] ListTags = ["audit-universe-objective-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseObjectiveModel, nameof(auditUniverseObjectiveModel));
        
        try
        {
            var entity = mapper.Map<AuditUniverseObjective>(auditUniverseObjectiveModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditUniverseObjective, createdEntity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditUniverseObjectiveTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
            
            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditUniverseObjective.", ex);
        }
    }
}
