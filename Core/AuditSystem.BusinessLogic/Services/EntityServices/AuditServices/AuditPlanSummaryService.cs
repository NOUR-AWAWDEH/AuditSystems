using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditPlanSummaryService(
    IRepository<Guid, AuditPlanSummary> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditPlanSummaryService
{
    public Task<Guid> CreateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel)
    {
        throw new NotImplementedException();
    }
}
