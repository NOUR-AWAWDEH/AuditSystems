using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class ChecklistService(
    IRepository<Guid, Checklist> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IChecklistService
{
    public Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel)
    {
        throw new NotImplementedException();
    }
}