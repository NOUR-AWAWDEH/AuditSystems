using AuditSystem.Application.Constants;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.FileStorage;
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
    IFileStorage fileStorage,
    IFilePathBuilder filePathBuilder,
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
    private static readonly string ServiceName = "SupportingDocs";
    private static readonly string[] SupportingDocTags = ["supporting-docs", "supporting-doc-list"];
    private static readonly string[] ListTags = ["supporting-doc-list"];
    private readonly IFileStorage _fileStorage = fileStorage; 
    private readonly IFilePathBuilder _filePathBuilder = filePathBuilder;


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
        if (supportingDocId == Guid.Empty)
            throw new ArgumentException("SupportingDocId cannot be empty", nameof(supportingDocId));

        var cacheKey = GetCacheKey(supportingDocId);

        var entity = await repository.GetByIdAsync(supportingDocId)
            ?? throw new KeyNotFoundException($"{ServiceName} with ID {supportingDocId} not found.");

        var fullFilePath = _filePathBuilder.GetFullPath(entity.UserId ,ServiceName,entity.FileName);
        var fileDeleteResult = await _fileStorage.DeleteFileAsync(fullFilePath);
        if (!fileDeleteResult.IsSuccess)
        {
            logger.LogWarning("Failed to delete physical file {FileName} for document {Id}: {Error}", entity.FileName, supportingDocId, string.Join("; ", fileDeleteResult.Errors));
        }
        await repository.DeleteAsync(supportingDocId);
        await ClearCacheAsync(cacheKey);
    }

    public async Task<List<SupportingDocModel>> GetSupportingDocsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty", nameof(userId));

        var cacheKey = $"{CacheKeys.SupportingDoc}:user:{userId}";

        // Try to get from cache
        var cachedDocs = await cacheService.GetAsync<List<SupportingDoc>>(cacheKey);
        if (cachedDocs is { Count: > 0 })
            return mapper.Map<List<SupportingDocModel>>(cachedDocs);

        // Fallback to DB
        var entities = await repository.GetAllAsync(doc => doc.UserId == userId);

        // Return empty list instead of throwing exception
        if (entities is not { Count: > 0 })
            return new List<SupportingDocModel>();

        // Cache the list result
        await cacheService.SetAsync(cacheKey, entities, tags: ListTags);

        return mapper.Map<List<SupportingDocModel>>(entities);
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
    
    public async Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SupportingDocModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<SupportingDocModel>(cacheKey, countCacheKey, page, name);
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