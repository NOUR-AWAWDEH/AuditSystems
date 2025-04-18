using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class InternalAuditConsolidationReportService(
    IRepository<Guid, InternalAuditConsolidationReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IInternalAuditConsolidationReportService
{
    private static readonly string[] InternalAuditConsolidationReportTags = ["internal-audit-consolidation-reports", "internal-audit-consolidation-report-list"];
    private static readonly string[] ListTags = ["internal-audit-consolidation-report-list"]; // Tags for collections only

    public async Task<Guid> CreateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));

        try
        {
            var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.InternalAuditConsolidationReport, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: InternalAuditConsolidationReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create InternalAuditConsolidationReport.", ex);
        }
    }
    public async Task DeleteInternalAuditConsolidationReportAsync(Guid internalAuditConsolidationReportId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(internalAuditConsolidationReportId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID {internalAuditConsolidationReportId} not found.");
            }

            await repository.DeleteAsync(internalAuditConsolidationReportId);

            var cacheKey = string.Format(CacheKeys.InternalAuditConsolidationReport, internalAuditConsolidationReportId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete InternalAuditConsolidationReport.", ex);
        }
    }
    public async Task<InternalAuditConsolidationReportModel> GetInternalAuditConsolidationReportByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.InternalAuditConsolidationReport, id);

            var internalAuditConsolidationReport = await cacheService.GetAsync<InternalAuditConsolidationReport>(cacheKey);

            if (internalAuditConsolidationReport != null)
            {
                return mapper.Map<InternalAuditConsolidationReportModel>(internalAuditConsolidationReport);
            }

            internalAuditConsolidationReport = await repository.GetByIdAsync(id);

            if (internalAuditConsolidationReport == null)
            {
                throw new KeyNotFoundException($"InternalAuditConsolidationReport with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: internalAuditConsolidationReport,
                tags: InternalAuditConsolidationReportTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<InternalAuditConsolidationReportModel>(internalAuditConsolidationReport);  
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get InternalAuditConsolidationReport by ID {id}.", ex);
        }
    }
    public async Task UpdateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));
        try
        {
            var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.InternalAuditConsolidationReport, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: InternalAuditConsolidationReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update InternalAuditConsolidationReport.", ex);
        }
    }
}
