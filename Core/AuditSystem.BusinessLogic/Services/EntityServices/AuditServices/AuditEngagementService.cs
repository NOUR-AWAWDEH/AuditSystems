using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditEngagementService(
    IRepository<Guid, AuditEngagement> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditEngagementService> logger
) : BaseEntityCacheService<AuditEngagement>(
        cacheService,
        logger,
        CacheKeys.AuditEngagement,
        AuditEngagementTags,
        ListTags
    ),
    IAuditEngagementService
{
    private static readonly string[] AuditEngagementTags = ["audit-engagements", "audit-engagment-list" ];
    private static readonly string[] ListTags = ["audit-engagement-list"];

    public async Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));

        try
        {
            var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditEngagementModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditEngagement");
            throw new InvalidOperationException("Failed to create AuditEngagement.", ex);
        }
    }

    public async Task DeleteAuditEngagementAsync(Guid auditEngagementId)
    {
        if (auditEngagementId == Guid.Empty)
        {
            throw new ArgumentException("Audit engagement ID cannot be empty.", nameof(auditEngagementId));
        }

        var cacheKey = GetCacheKey(auditEngagementId);

        try
        {
            var entity = await repository.GetByIdAsync(auditEngagementId)
                         ?? throw new KeyNotFoundException($"AuditEngagement with ID {auditEngagementId} not found.");

            await repository.DeleteAsync(auditEngagementId);
            await ClearCacheAsync(cacheKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditEngagement with ID {AuditEngagementId}", auditEngagementId);
            throw new InvalidOperationException($"Failed to delete AuditEngagement with ID {auditEngagementId}", ex);
        }
    }

    public async Task<AuditEngagementModel> GetAuditEngagementByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditEngagement>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditEngagementModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"AuditEngagement with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);

            return mapper.Map<AuditEngagementModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditEngagement with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditEngagement with ID '{id}'", ex);
        }
    }

    public async Task UpdateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));

        try
        {
            var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditEngagement with ID {Id}", auditEngagementModel.Id);
            throw new InvalidOperationException("Failed to update AuditEngagement.", ex);
        }
    }
}
