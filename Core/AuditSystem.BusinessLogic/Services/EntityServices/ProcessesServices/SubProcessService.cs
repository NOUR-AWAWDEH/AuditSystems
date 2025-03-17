using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubProcessService
{
    public Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel)
    {
        throw new NotImplementedException();
    }
}
