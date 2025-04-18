using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class ReportingFollowUpService(
    IRepository<Guid, ReportingFollowUp> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IReportingFollowUpService
{
    private static readonly string[] ReportingFollowUpTags = ["reporting-follow-ups", "reporting-follow-up-list"];
    private static readonly string[] ListTags = ["reporting-follow-up-list"]; // Tags for collections only

    public async Task<Guid> CreateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        ArgumentNullException.ThrowIfNull(reportingFollowUpModel, nameof(reportingFollowUpModel));

        try
        {
            var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.ReportingFollowUp, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ReportingFollowUpTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ReportingFollowUp.", ex);
        }
    }
    public async Task DeleteReportingFollowUpAsync(Guid reportingFollowUpId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(reportingFollowUpId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"ReportingFollowUp with ID {reportingFollowUpId} not found.");
            }

            await repository.DeleteAsync(reportingFollowUpId);
            
            var cacheKey = string.Format(CacheKeys.ReportingFollowUp, reportingFollowUpId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete ReportingFollowUp.", ex);
        }
    }
    public async Task<ReportingFollowUpModel> GetReportingFollowUpByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.ReportingFollowUp, id);
            
            var cachedEntity = await cacheService.GetAsync<ReportingFollowUp>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<ReportingFollowUpModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"ReportingFollowUp with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ReportingFollowUpTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<ReportingFollowUpModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get ReportingFollowUp by ID {id}.", ex);
        }
    }
    public async Task UpdateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        ArgumentNullException.ThrowIfNull(reportingFollowUpModel, nameof(reportingFollowUpModel));

        try
        {
            var entity = mapper.Map<ReportingFollowUp>(reportingFollowUpModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.ReportingFollowUp, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ReportingFollowUpTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update ReportingFollowUp.", ex);
        }
    }
}