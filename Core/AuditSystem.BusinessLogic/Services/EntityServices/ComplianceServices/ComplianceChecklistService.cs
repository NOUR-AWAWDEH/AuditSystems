using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceChecklistService(
    IRepository<Guid, ComplianceChecklist> repository,
    IMapper mapper,
    ICacheService cacheService
    )
    : IComplianceChecklistService
{
    private static readonly string[] ComplianceChecklistTags = ["compliance-checklists", "compliance-checklist-list"];
    private static readonly string[] ListTags = ["compliance-checklist-list"]; // Tags for collections only

    public async Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        try
        {
            var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.ComplianceChecklist, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ComplianceChecklistTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ComplianceChecklist.", ex);
        }
    }

    public async Task DeleteComplianceChecklistAsync(Guid complianceChecklistId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(complianceChecklistId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"ComplianceChecklist with ID {complianceChecklistId} not found.");
            }
            
            await repository.DeleteAsync(complianceChecklistId);

            var cacheKey = string.Format(CacheKeys.ComplianceChecklist, complianceChecklistId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete ComplianceChecklist.", ex);
        }
    }

    public Task<ComplianceChecklistModel> GetComplianceChecklistByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        try
        {
            var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.ComplianceChecklist, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: ComplianceChecklistTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update ComplianceChecklist.", ex);
        }
    }
}
