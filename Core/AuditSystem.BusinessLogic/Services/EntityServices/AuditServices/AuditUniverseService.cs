using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseService
{
    public Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        throw new NotImplementedException();
    }
}
