using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Checklists;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;

internal sealed class ChecklistService(
    IRepository<Guid, Checklist> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ChecklistService> logger) :
    BaseEntityCacheService<Checklist>(
        cacheService,
        logger,
        CacheKeys.Checklist,
        ChecklistTags,
        ListTags
    ),
    IChecklistService
{
    private static readonly string[] ChecklistTags = ["checklists", "checklist-list"];
    private static readonly string[] ListTags = ["checklist-list"];
    
    public async Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        try
        {
            var entity = mapper.Map<Checklist>(checklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for ChecklistModel");
            throw new ArgumentException("Mapping failed for ChecklistModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating Checklist.");
            throw new InvalidOperationException("An error occurred while creating Checklist.", ex);
        }
    }
    
    public async Task DeleteChecklistAsync(Guid checklistId)
    {
        var cacheKey = GetCacheKey(checklistId);

        try
        {
            var entity = await repository.GetByIdAsync(checklistId)
                ?? throw new KeyNotFoundException($"Checklist with ID {checklistId} not found.");

            await repository.DeleteAsync(checklistId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Checklist with ID: {id} not found.", checklistId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Checklist with ID: {id}.", checklistId);
            throw new InvalidOperationException("Failed to delete Checklist.", ex);
        }
    }
    
    public async Task<ChecklistModel> GetChecklistByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<Checklist>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<ChecklistModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Checklist with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<ChecklistModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Checklist with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Checklist with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<ChecklistModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateChecklistAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        try
        {
            var entity = mapper.Map<Checklist>(checklistModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Checklist with ID {Id}", checklistModel.Id);
            throw new InvalidOperationException($"Failed to update Checklist with ID '{checklistModel.Id}'", ex);
        }
    }
}