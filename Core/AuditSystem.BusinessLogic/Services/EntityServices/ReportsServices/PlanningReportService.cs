﻿using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class PlanningReportService(
    IRepository<Guid, PlanningReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IPlanningReportService
{
    private static readonly string[] PlanningReportTags = ["planning-reports", "planning-report-list"];
    private static readonly string[] ListTags = ["planning-report-list"]; // Tags for collections only

    public async Task<Guid> CreatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));

        try
        {
            var entity = mapper.Map<PlanningReport>(planningReportModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.PlanningReport, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: PlanningReportTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create PlanningReport.", ex);
        }
    }
    public async Task DeletePlanningReportAsync(Guid planningReportId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(planningReportId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"PlanningReport with ID {planningReportId} not found.");
            }

            await repository.DeleteAsync(planningReportId);

            var cacheKey = string.Format(CacheKeys.PlanningReport, planningReportId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete PlanningReport.", ex);
        }
    }
    public async Task<PlanningReportModel> GetPlanningReportByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.PlanningReport, id);

            var cachedEntity = await cacheService.GetAsync<PlanningReport>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<PlanningReportModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"PlanningReport with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: PlanningReportTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<PlanningReportModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get PlanningReport by ID {id}.", ex);
        }  
    }
    public async Task UpdatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));
        
        try
        {
            var entity = mapper.Map<PlanningReport>(planningReportModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.PlanningReport, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: PlanningReportTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update PlanningReport.", ex);
        }
    }
}
