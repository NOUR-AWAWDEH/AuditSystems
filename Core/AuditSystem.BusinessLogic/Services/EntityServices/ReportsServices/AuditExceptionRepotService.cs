using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class AuditExceptionRepotService(
    IRepository<Guid, AuditExceptionReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditExceptionRepotService
{
    private static readonly string[] AuditExceptionReportTags = ["audit-exception-reports", "audit-exception-report-list"];
    private static readonly string[] ListTags = ["audit-exception-report-list"]; // Tags for collections only

    public async Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));

        try
        {
            var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditExceptionReport, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: AuditExceptionReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create AuditExceptionReport.", ex);
        }
    }

    public async Task UpdateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));

        try
        {
            var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);

            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.AuditExceptionReport, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditExceptionReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update AuditExceptionReport.", ex);
        }
    }
}
