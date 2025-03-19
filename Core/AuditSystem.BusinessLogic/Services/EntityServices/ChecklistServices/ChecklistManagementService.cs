using AuditSystem.Application.Constants;
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
    private static readonly string[] ChecklistManagement = ["checklist-management", "checklist-management-list"];
    private static readonly string[] ListTags = ["checklist-management-list"]; // Tags for collections only
    
    public async Task<Guid> CreateChecklistAsync(ChecklistManagementModel checklistModel)
    {
        ArgumentNullException.ThrowIfNull(checklistModel, nameof(checklistModel));
        
        try
        {
            var entity = mapper.Map<ChecklistManagement>(checklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.CacheKey, CacheKeys.ChecklistManagement, createdEntity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ChecklistManagement,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
            
            return createdEntity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ChecklistManagement.", ex);
        }
    }
}