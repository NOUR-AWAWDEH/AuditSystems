using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseObjectiveService(
    IRepository<Guid, AuditUniverseObjective> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseObjectiveService
{
    public Task<Guid> CreateAuditUniverseObjectiveAsync(AuditUniverseObjectiveModel auditUniverseObjectiveModel)
    {
        throw new NotImplementedException();
    }
}
