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
    public async Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));
        
        try
        {
            var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
