using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceChecklistService(
    IRepository<Guid, ComplianceChecklist> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ComplianceChecklistService> logger) :
    UnifiedCacheService<Guid, ComplianceChecklist, ComplianceChecklistModel>(
        repository,
        mapper,
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
      
        var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        return createdEntity.Id;
    }
    
    public async Task DeleteComplianceChecklistAsync(Guid complianceChecklistId)
    {
        var cacheKey = GetCacheKey(complianceChecklistId);
        
        var entity = await repository.GetByIdAsync(complianceChecklistId)
            ?? throw new KeyNotFoundException($"ComplianceChecklist with ID {complianceChecklistId} not found.");

        await repository.DeleteAsync(complianceChecklistId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<ComplianceChecklistModel> GetComplianceChecklistByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<ComplianceChecklist>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<ComplianceChecklistModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"ComplianceChecklist with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<ComplianceChecklistModel>(entity);
    }

    public Task<PaginationResult<ComplianceChecklistModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
