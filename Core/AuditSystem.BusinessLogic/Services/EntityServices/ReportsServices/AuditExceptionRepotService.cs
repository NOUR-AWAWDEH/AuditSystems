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
    public Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        throw new NotImplementedException();
    }
}
