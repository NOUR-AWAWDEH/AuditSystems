using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
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
    UnifiedCacheService<Guid, TaskManagement, TaskManagementModel>(
        repository,
        mapper,
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

        var entity = mapper.Map<TaskManagement>(taskManagementModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteTaskAsync(Guid taskManagementId)
    {
        var cacheKey = GetCacheKey(taskManagementId);
        
        var entity = await repository.GetByIdAsync(taskManagementId)
            ?? throw new KeyNotFoundException($"TaskManagement with ID {taskManagementId} not found.");

        await repository.DeleteAsync(taskManagementId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<TaskManagementModel> GetTaskByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var entity = await cacheService.GetAsync<TaskManagement>(cacheKey);
        if (entity != null)
            return mapper.Map<TaskManagementModel>(entity);

        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"TaskManagement with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<TaskManagementModel>(entity);
    }

    public Task<PaginationResult<TaskManagementModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateTaskAsync(TaskManagementModel taskManagementModel)
    {
        ArgumentNullException.ThrowIfNull(taskManagementModel, nameof(taskManagementModel));
        
        var entity = mapper.Map<TaskManagement>(taskManagementModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}