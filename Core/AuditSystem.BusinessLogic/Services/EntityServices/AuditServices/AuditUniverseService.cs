using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseService
{
    private static readonly string[] AuditUniverseTags = ["audit-universes", "audit-universe-list"];
    private static readonly string[] ListTags = ["audit-universe-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));
        
        try
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.AuditUniverse, createdEntity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditUniverseTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
            
            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditUniverse.", ex);
        }
    }
}
