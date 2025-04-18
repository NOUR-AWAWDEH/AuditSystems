﻿using AuditSystem.Application.Constants;
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
    : IAuditExceptionReportService
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
    public async Task DeleteAuditExceptionReportAsync(Guid auditExceptionReportId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(auditExceptionReportId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditExceptionReport with ID {auditExceptionReportId} not found.");
            }

            await repository.DeleteAsync(auditExceptionReportId);

            var cacheKey = string.Format(CacheKeys.AuditExceptionReport, auditExceptionReportId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete AuditExceptionReport.", ex);
        }
    }
    public async Task<AuditExceptionReportModel> GetAuditExceptionReportByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.AuditExceptionReport, id);

            var cachedEntity = await cacheService.GetAsync<AuditExceptionReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditExceptionReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"AuditExceptionReport with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: AuditExceptionReportTags,
                expiration: CacheExpirations.MediumTerm
            ); 

            return mapper.Map<AuditExceptionReportModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AuditExceptionReport by ID {id}.", ex);
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
