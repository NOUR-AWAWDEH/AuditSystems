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
}
