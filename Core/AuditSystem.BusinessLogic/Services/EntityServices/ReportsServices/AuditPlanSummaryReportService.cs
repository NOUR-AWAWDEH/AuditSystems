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
    public Task<Guid> CreateAuditPlanSummaryReportAsync(AuditPlanSummaryReportModel auditPlanSummaryReportModel)
    {
        throw new NotImplementedException();
    }
}
