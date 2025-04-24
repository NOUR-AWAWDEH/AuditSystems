using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;

internal sealed class SupportingDocService(
    IRepository<Guid, SupportingDoc> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SupportingDocService> logger) : 
    UnifiedCacheService<Guid, SupportingDoc, SupportingDocModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.SupportingDoc,
        SupportingDocTags,
        ListTags
    ),
    ISupportingDocService
{
    private static readonly string[] SupportingDocTags = ["supporting-docs", "supporting-doc-list"];
    private static readonly string[] ListTags = ["supporting-doc-list"];

    public async Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));
       
        var entity = mapper.Map<SupportingDoc>(supportingDocModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }

    public async Task DeleteSupportingDocAsync(Guid supportingDocId)
    {
        var cacheKey = GetCacheKey(supportingDocId);

        var entity = await repository.GetByIdAsync(supportingDocId)
            ?? throw new KeyNotFoundException($"SupportingDoc with ID {supportingDocId} not found.");

        await repository.DeleteAsync(supportingDocId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        var entity = await cacheService.GetAsync<SupportingDoc>(cacheKey);
        if (entity != null)
            return mapper.Map<SupportingDocModel>(entity);
        
        entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"SupportingDoc with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<SupportingDocModel>(entity);
    }

    public Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));
        
        var entity = mapper.Map<SupportingDoc>(supportingDocModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}