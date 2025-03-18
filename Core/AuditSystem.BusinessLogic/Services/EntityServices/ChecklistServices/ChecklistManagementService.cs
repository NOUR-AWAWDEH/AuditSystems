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
    public async Task<Guid> CreateChecklistAsync(ChecklistManagementModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));
        
        try
        {
            var entity = mapper.Map<ChecklistManagement>(checklistModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}