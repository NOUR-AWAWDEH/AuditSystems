using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Domain.Entities.Tasks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.TasksServices;

internal sealed class TaskManagementService(
    IRepository<Guid, TaskManagement> repository,
    IMapper mapper,
    ICacheService cacheService) 
    : ITaskManagementService
{
    public async Task<Guid> CreateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));

        try
        {
            var entity = mapper.Map<TaskManagement>(taskManagementModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}