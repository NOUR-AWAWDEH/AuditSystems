using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceChecklistService(
    IRepository<Guid, ComplianceChecklist> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ComplianceChecklistService> logger) :
    BaseEntityCacheService<ComplianceChecklist>(
        cacheService,
        logger,
        CacheKeys.ComplianceChecklist,
        ComplianceChecklistTags,
        ListTags
    ),
    IComplianceChecklistService
{
    private static readonly string[] ComplianceChecklistTags = ["compliance-checklists", "compliance-checklist-list"];
    private static readonly string[] ListTags = ["compliance-checklist-list"];

    public async Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        try
        {
            var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, entity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for ComplianceChecklistModel");
            throw new ArgumentException("Mapping failed for ComplianceChecklistModel", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating ComplianceChecklist.");
            throw new InvalidOperationException("An error occurred while creating ComplianceChecklist.", ex);
        }
    }
    
    public async Task DeleteComplianceChecklistAsync(Guid complianceChecklistId)
    {
        var cacheKey = GetCacheKey(complianceChecklistId);
        
        try
        {
            var entity = await repository.GetByIdAsync(complianceChecklistId)
                ?? throw new KeyNotFoundException($"ComplianceChecklist with ID {complianceChecklistId} not found.");

            await repository.DeleteAsync(complianceChecklistId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "ComplianceChecklist with ID {ComplianceChecklistId} not found", complianceChecklistId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete ComplianceChecklist ID {ComplianceChecklistId}", complianceChecklistId);
            throw new InvalidOperationException($"Failed to delete ComplianceChecklist: {complianceChecklistId}", ex);
        }
    }
    
    public async Task<ComplianceChecklistModel> GetComplianceChecklistByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<ComplianceChecklist>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<ComplianceChecklistModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"ComplianceChecklist with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<ComplianceChecklistModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve ComplianceChecklist with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve ComplianceChecklist with ID '{id}'", ex);
        }
    }
    
    public async Task UpdateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        try
        {
            var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update ComplianceChecklist with ID {Id}", complianceChecklistModel.Id);
            throw new InvalidOperationException($"Failed to update ComplianceChecklist with ID '{complianceChecklistModel.Id}'", ex);
        }
    }
}
