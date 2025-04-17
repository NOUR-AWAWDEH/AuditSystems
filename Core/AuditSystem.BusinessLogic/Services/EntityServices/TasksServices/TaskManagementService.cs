using AuditSystem.Application.Constants;
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
    private static readonly string[] TaskManagementTags = ["task-managements", "task-management-list"];
    private static readonly string[] ListTags = ["task-management-list"]; // Tags for collections only

    public async Task<Guid> CreateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));

        try
        {
            var entity = mapper.Map<TaskManagement>(taskManagementModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.TaskManagement, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: TaskManagementTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create TaskManagement.", ex);
        }
    }

    public async Task DeleteTaskAsync(Guid taskManagementId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(taskManagementId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"TaskManagement with ID {taskManagementId} not found.");
            }

            await repository.DeleteAsync(taskManagementId);

            var cacheKey = string.Format(CacheKeys.TaskManagement, taskManagementId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete TaskManagement.", ex);
        }

    }

    public Task<TaskManagementModel> GetTaskByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));

        try
        {
            var entity = mapper.Map<TaskManagement>(taskManagementModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.TaskManagement, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: TaskManagementTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update TaskManagement.", ex);
        }
    }
}