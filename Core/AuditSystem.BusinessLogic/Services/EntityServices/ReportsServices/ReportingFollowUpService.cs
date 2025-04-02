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

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.ReportingFollowUp, createdEntity.Id);

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
}