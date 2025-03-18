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
    public async Task<Guid> CreatePlanningReportAsync(PlanningReportModel planningReportModel)
    {
        ArgumentNullException.ThrowIfNull(planningReportModel, nameof(planningReportModel));
        
        try
        {
            var entity = mapper.Map<PlanningReport>(planningReportModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
