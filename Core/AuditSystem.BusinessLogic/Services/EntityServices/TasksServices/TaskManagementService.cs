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
    public Task<Guid> CreateTaskAsync(TaskManagementModel taskManagementModel)
    {
        throw new NotImplementedException();
    }
}