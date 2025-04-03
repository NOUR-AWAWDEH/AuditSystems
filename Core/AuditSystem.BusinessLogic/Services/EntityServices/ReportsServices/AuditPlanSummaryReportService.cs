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
}
