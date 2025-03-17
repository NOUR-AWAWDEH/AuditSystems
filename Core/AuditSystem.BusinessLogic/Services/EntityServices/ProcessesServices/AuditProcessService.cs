using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditProcessService
{
    public Task<Guid> CreateAuditProcessAsync(AuditProcessModel processModel)
    {
        throw new NotImplementedException();
    }
}
