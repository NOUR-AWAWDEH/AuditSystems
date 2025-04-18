using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class AuditPlanSummaryReportService(
    IRepository<Guid, AuditPlanSummaryReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditPlanSummaryReportService
{
    private static readonly string[] AuditPlanSummaryReportTags = ["audit-plan-summary-reports", "audit-plan-summary-report-list"];
    private static readonly string[] ListTags = ["audit-plan-summary-report-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditPlanSummaryReport, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditPlanSummaryReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditPlanSummaryReport.", ex);
        }
    }
    public async Task DeleteAuditPlanSummaryReportAsync(Guid auditPlanSummaryReportId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(auditPlanSummaryReportId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditPlanSummaryReport with ID {auditPlanSummaryReportId} not found.");
            }

            await repository.DeleteAsync(auditPlanSummaryReportId);

            var cacheKey = string.Format(CacheKeys.AuditPlanSummaryReport, auditPlanSummaryReportId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditPlanSummaryReport.", ex);
        }
    }
    public async Task<AuditPlanSummaryReportModel> GetAuditPlanSummaryReportByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditPlanSummaryReport, id);

            var cachedEntity = await cacheService.GetAsync<AuditPlanSummaryReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditPlanSummaryReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditPlanSummaryReport with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditPlanSummaryReportTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<AuditPlanSummaryReportModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditPlanSummaryReport by ID {id}.", ex);
        }
    }
    public async Task UpdateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));

        try
        {
            var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
            
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.AuditPlanSummaryReport, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditPlanSummaryReportTags,
                expiration: CacheExpirations.MediumTerm
            );
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditPlanSummaryReport.", ex);
        }
    }
}
