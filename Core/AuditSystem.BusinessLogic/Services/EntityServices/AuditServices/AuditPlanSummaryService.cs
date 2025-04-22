using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditPlanSummaryService(
    IRepository<Guid, AuditPlanSummary> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditPlanSummaryService> logger) : 
    BaseEntityCacheService<AuditPlanSummary>(
        cacheService,
        logger,
        CacheKeys.AuditPlanSummary,
        AuditPlanSummaryListKeys,
        ListTags
    ),
    IAuditPlanSummaryService
{
    private static readonly string[] AuditPlanSummaryListKeys = ["audit-plan-summaries", "audit-plan-summary-list"];
    private static readonly string[] ListTags = ["audit-plan-summary-list"];

    public async Task<Guid> CreateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditPlanSummaryModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditPlanSummary");
            throw new InvalidOperationException("Failed to create AuditPlanSummary.", ex);
        }
    }

    public async Task DeleteAuditPlanSummaryAsync(Guid auditPlanSummaryId)
    {
        var cacheKey = GetCacheKey(auditPlanSummaryId);

        try
        {
            var entity = await repository.GetByIdAsync(auditPlanSummaryId) ?? 
                throw new KeyNotFoundException($"AuditPlanSummary with ID {auditPlanSummaryId} not found.");

            await repository.DeleteAsync(auditPlanSummaryId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "AuditPlanSummary with ID {Id} not found", auditPlanSummaryId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditPlanSummary with ID {Id}", auditPlanSummaryId);
            throw new InvalidOperationException($"Failed to delete AuditPlanSummary with ID {auditPlanSummaryId}", ex);
        }
    }

    public async Task<AuditPlanSummaryModel> GetAuditPlanSummaryByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditPlanSummary>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditPlanSummaryModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id) ?? 
                throw new KeyNotFoundException($"AuditPlanSummary with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);

            return mapper.Map<AuditPlanSummaryModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditPlanSummary with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditPlanSummary with ID '{id}'", ex);
        }
    }

    public async Task UpdateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditPlanSummary with ID {Id}", auditPlanSummaryModel.Id);
            throw new InvalidOperationException("Failed to update AuditPlanSummary.", ex);
        }
    }
}