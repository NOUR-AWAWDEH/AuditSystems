using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class RemarkService(
    IRepository<Guid, Remark> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRemarkService
{
    public Task<Guid> CreateRemarkAsync(RemarkModel remarkModel)
    {
        throw new NotImplementedException();
    }
}
