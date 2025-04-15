using AuditSystem.Application.Constants;
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
    private static readonly string[] ChecklistTags = ["checklists", "checklist-list"];
    private static readonly string[] ListTags = ["checklist-list"]; // Tags for collections only

    public async Task<Guid> CreateCheckListAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        try
        {
            var entity = mapper.Map<Checklist>(checklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Checklist, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ChecklistTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Checklist.", ex);
        }
    }

    public async Task DeleteChecklistAsync(Guid checklistId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(checklistId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Checklist with ID {checklistId} not found.");
            }

            await repository.DeleteAsync(checklistId);

            var cacheKey = string.Format(CacheKeys.Checklist, checklistId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Checklist.", ex);
        }
    }

    public async Task UpdateChecklistAsync(ChecklistModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));

        try
        {
            var entity = mapper.Map<Checklist>(checklistModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Checklist, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ChecklistTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Checklist.", ex);
        }
    }
}