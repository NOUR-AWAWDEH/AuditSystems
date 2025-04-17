using AuditSystem.Contract.Models.Tasks;

namespace AuditSystem.Contract.Interfaces.ModelServices.TasksServices;

public interface ITaskManagementService
{
    public Task<Guid> CreateTaskAsync(TaskManagementModel taskManagementModel);
    public Task UpdateTaskAsync(TaskManagementModel taskManagementModel);
    public Task DeleteTaskAsync(Guid taskManagementId);
    public Task<TaskManagementModel> GetTaskByIdAsync(Guid Id);
}