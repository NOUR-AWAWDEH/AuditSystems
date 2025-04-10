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
}