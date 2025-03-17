using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class ChecklistManagementService(
    IRepository<Guid, ChecklistManagement> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IChecklistManagementService
{
    public Task<Guid> CreateChecklistAsync(ChecklistManagementModel checklistModel)
    {
        throw new NotImplementedException();
    }
}