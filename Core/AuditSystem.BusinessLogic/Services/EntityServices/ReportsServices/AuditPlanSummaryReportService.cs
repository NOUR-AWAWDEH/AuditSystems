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
    public async Task<Guid> CreateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditPlanSummaryReportModel, nameof(auditPlanSummaryReportModel));
        
        try
        {
            var entity = mapper.Map<AuditPlanSummaryReport>(auditPlanSummaryReportModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
