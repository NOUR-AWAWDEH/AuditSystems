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

    public async Task DeleteAuditEngagementAsync(Guid auditEngagementId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(auditEngagementId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditEngagement with ID {auditEngagementId} not found.");
            }

            await repository.DeleteAsync(auditEngagementId);

            var cacheKey = string.Format(CacheKeys.AuditEngagement, auditEngagementId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditEngagement.", ex);
        }
    }

    public async Task UpdateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));
        
        try 
        {
            var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditEngagement, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditEngagementServiceTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditEngagement.", ex);
        }
    }
}
