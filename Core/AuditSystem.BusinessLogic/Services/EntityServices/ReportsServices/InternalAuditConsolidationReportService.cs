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
    public async Task<Guid> CreateInternalAuditConsolidationReportAsync(InternalAuditConsolidationReportModel internalAuditConsolidationReportModel)
    {
        ArgumentNullException.ThrowIfNull(internalAuditConsolidationReportModel, nameof(internalAuditConsolidationReportModel));
        
        try
        {
            var entity = mapper.Map<InternalAuditConsolidationReport>(internalAuditConsolidationReportModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
