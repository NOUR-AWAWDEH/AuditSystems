using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditPlanSummaryService(
    IRepository<Guid, AuditPlanSummary> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditPlanSummaryService
{
    private static readonly string[] AuditPlanSummaryTags = ["audit-plan-summaries", "audit-plan-summary-list"];
    private static readonly string[] ListTags = ["audit-plan-summary-list"]; // Tags for collections only
    public async Task<Guid> CreateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditPlanSummary, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditPlanSummaryTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditPlanSummary.", ex);
        }
    }
    public async Task DeleteAuditPlanSummaryAsync(Guid auditPlanSummaryId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(auditPlanSummaryId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditPlanSummary with ID {auditPlanSummaryId} not found.");
            }
            
            await repository.DeleteAsync(auditPlanSummaryId);
            
            var cacheKey = string.Format(CacheKeys.AuditPlanSummary, auditPlanSummaryId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditPlanSummary.", ex);
        }
    }
    public async Task<AuditPlanSummaryModel> GetAuditPlanSummaryByIdAsync(Guid Id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditPlanSummary, Id);
            
            var cachedModel = await cacheService.GetAsync<AuditPlanSummary>(cacheKey);
            if (cachedModel != null)
            {
                return mapper.Map<AuditPlanSummaryModel>(cachedModel);
            }

            var entity = await repository.GetByIdAsync(Id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditPlanSummary with ID {Id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditPlanSummaryTags,
                expiration: CacheExpirations.MediumTerm
                );

            return mapper.Map<AuditPlanSummaryModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditPlanSummary ID {Id}.", ex);
        }
    }
    public async Task UpdateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryModel, nameof(auditPlanSummaryModel));
        try
        {
            var entity = mapper.Map<AuditPlanSummary>(auditPlanSummaryModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditPlanSummary, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditPlanSummaryTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditPlanSummary.", ex);
        }
    }
}
