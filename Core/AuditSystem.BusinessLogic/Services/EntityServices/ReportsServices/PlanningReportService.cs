using AuditSystem.Application.Constants;
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
}
