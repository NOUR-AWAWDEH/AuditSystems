using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Domain.Entities.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.TasksServices;

internal sealed class TaskManagementService(
    IRepository<Guid, TaskManagement> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<TaskManagementService> logger) :
    BaseEntityCacheService<TaskManagement>(
        cacheService,
        logger,
        CacheKeys.TaskManagement,
        TaskManagementTags,
        ListTags
    ), 
    ITaskManagementService
{
    private static readonly string[] TaskManagementTags = ["task-managements", "task-management-list"];
    private static readonly string[] ListTags = ["task-management-list"];

    public async Task<Guid> CreateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));

        try
        {
            var entity = mapper.Map<TaskManagement>(taskManagementModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for TaskManagementModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create TaskManagement");
            throw new InvalidOperationException("Failed to create TaskManagement.", ex);
        }
    }

    public async Task DeleteTaskAsync(Guid taskManagementId)
    {
        var cacheKey = GetCacheKey(taskManagementId);
        
        try
        {
            var entity = await repository.GetByIdAsync(taskManagementId)
                ?? throw new KeyNotFoundException($"TaskManagement with ID {taskManagementId} not found.");

            await repository.DeleteAsync(taskManagementId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "TaskManagement with ID {TaskManagementId} not found", taskManagementId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete TaskManagement ID {TaskManagementId}", taskManagementId);
            throw new InvalidOperationException($"Failed to delete TaskManagement: {taskManagementId}", ex);
        }
    }

    public async Task<TaskManagementModel> GetTaskByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<TaskManagement>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<TaskManagementModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"TaskManagement with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<TaskManagementModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve TaskManagement with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve TaskManagement with ID '{id}'", ex);
        }
    }

    public async Task UpdateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));
        
        try
        {
            var entity = mapper.Map<TaskManagement>(taskManagementModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update TaskManagement with ID {Id}", taskManagementModel.Id);
            throw new InvalidOperationException($"Failed to update TaskManagement with ID '{taskManagementModel.Id}'", ex);
        }
    }
}