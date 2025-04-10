using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditEngagementService(
    IRepository<Guid, AuditEngagement> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditEngagementService
{
    private static readonly string[] AuditEngagementServiceTags = ["audit-engagements", "audit-engagement-list"];
    private static readonly string[] ListTags = ["audit-engagement-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));

        try
        {
            var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditEngagement, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditEngagementServiceTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditEngagement.", ex);
        }

    }
}
