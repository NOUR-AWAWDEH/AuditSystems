using Ardalis.Result;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.SupportingDocs;
using AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;

internal sealed class SupportingDocService(
    IRepository<Guid, SupportingDoc> repository, 
    IMapper mapper, 
    ICacheService cacheService, 
    IFileStorage fileStorage, 
    IFilePathBuilder filePathBuilder, 
    ILogger<SupportingDocService> _logger,
    ISupportingDocValidation validationHelper,
    ISupportingDocSearch searchHelper) 
    : UnifiedCacheService<Guid, SupportingDoc, SupportingDocModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.SupportingDoc,
        SupportingDocsConstants.SupportingDocTags,
        SupportingDocsConstants.ListTags
    ), ISupportingDocService
{
    private readonly ISupportingDocValidation _validationHelper = validationHelper;
    private readonly ISupportingDocSearch _searchHelper = searchHelper;
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly IFilePathBuilder _filePathBuilder = filePathBuilder;

    public async Task<Result<Guid>> CreateSupportingDocAsync(IFormFile file, string fileName, Guid userId)
    {
        SupportingDocValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Creating supporting document for user {UserId}", userId);

        // Validate file
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Empty file received for user {UserId}", userId);
            return Result.Error("File cannot be empty");
        }

        // Save file to storage
        var fileSaveResult = await _fileStorage.SaveFileAsync(file, userId, SupportingDocsConstants.ServiceName, fileName);
        if (!fileSaveResult.IsSuccess)
        {
            _logger.LogError("Failed to save file for user {UserId}: {Errors}", userId, string.Join(", ", fileSaveResult.Errors));
            return Result.Error(new ErrorList(fileSaveResult.Errors));
        }

        // Create supporting document model
        var supportingDocModel = new SupportingDocModel
        {
            Id = Guid.NewGuid(),
            FileName = fileSaveResult.Value.SavedFileName,
            FileSize = fileSaveResult.Value.FileSize,
            DriveFileId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Create supporting document entity
        var entity = mapper.Map<SupportingDoc>(supportingDocModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created supporting document {SupportingDocId} for user {UserId}", 
            createdEntity.Id, userId);
        
        return Result.Created(createdEntity.Id);
    }

    public async Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id, Guid userId)
    {
        SupportingDocValidationHelper.ValidateUserId(userId);
        SupportingDocValidationHelper.ValidateSupportingDocId(id);
        
        _logger.LogDebug("Retrieving supporting document {SupportingDocId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<SupportingDocModel>(
            id,
            userId,
            SupportingDocsConstants.SupportingDocOwnerIdSelector,
            SupportingDocsConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved supporting document {SupportingDocId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateSupportingDocOwnershipAsync(id, userId);
        
        await CacheEntityAsync(entity);
        
        return mapper.Map<SupportingDocModel>(entity);
    }

    public async Task DeleteSupportingDocAsync(Guid supportingDocId, Guid userId)
    {
        SupportingDocValidationHelper.ValidateUserId(userId);
        SupportingDocValidationHelper.ValidateSupportingDocId(supportingDocId);
        
        _logger.LogInformation("Deleting supporting document {SupportingDocId} for user {UserId}", supportingDocId, userId);

        var entity = await _validationHelper.ValidateSupportingDocOwnershipAsync(supportingDocId, userId);
        
        var fullFilePath = _filePathBuilder.GetFullPath(entity.UserId, SupportingDocsConstants.ServiceName, entity.FileName);
        var fileDeleteResult = await _fileStorage.DeleteFileAsync(fullFilePath);
        if (!fileDeleteResult.IsSuccess)
        {
            _logger.LogWarning("Failed to delete physical file {FileName} for document {Id}: {Error}", 
                entity.FileName, supportingDocId, string.Join("; ", fileDeleteResult.Errors));
        }
        
        await repository.DeleteAsync(supportingDocId);
        
        await ClearEntityCacheAsync(supportingDocId);
        
        await ClearUserRelatedCachesAsync(userId, SupportingDocsConstants.EntityName);
        
        _logger.LogInformation("Successfully deleted supporting document {SupportingDocId}", supportingDocId);
    }

    public async Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));
        SupportingDocValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Updating supporting document {SupportingDocId} for user {UserId}", 
            supportingDocModel.Id, userId);
    
        var existingEntity = await _validationHelper.ValidateSupportingDocOwnershipAsync(supportingDocModel.Id, userId);
    
        mapper.Map(supportingDocModel, existingEntity);
        
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
        
        _logger.LogInformation("Successfully updated supporting document {SupportingDocId}", supportingDocModel.Id);
    }

    public async Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        _logger.LogDebug("Searching supporting documents with name filter: {Name}", name ?? "(none)");
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SupportingDocModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved search results from cache");
            return cachedResult;
        }

        return await GetFromRepositoryAsync<SupportingDocModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<SupportingDocModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page page)
    {
        SupportingDocValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        _logger.LogDebug("Searching supporting documents for user {UserId} with term: {SearchTerm}", 
            userId, searchTerm ?? "(none)");
        
        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, page);
    
        var cachedResult = await GetFromCacheAsync<SupportingDocModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved user search results from cache");
            return cachedResult;
        }
    
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task<IReadOnlyList<SupportingDocModel>> GetSupportingDocsByUserIdAsync(Guid userId)
    {
        SupportingDocValidationHelper.ValidateUserId(userId);
        
        _logger.LogDebug("Retrieving all supporting documents for user {UserId}", userId);

        var cacheKey = $"{CacheKeys.SupportingDoc}:user:{userId}";

        var cachedDocs = await cacheService.GetAsync<List<SupportingDoc>>(cacheKey);
        if (cachedDocs is { Count: > 0 })
        {
            _logger.LogDebug("Retrieved {Count} supporting documents from cache for user {UserId}", cachedDocs.Count, userId);
            return mapper.Map<List<SupportingDocModel>>(cachedDocs);
        }

        var entities = await repository.GetAllAsync(doc => doc.UserId == userId);

        if (entities is not { Count: > 0 })
        {
            _logger.LogDebug("No supporting documents found for user {UserId}", userId);
            return [];
        }

        await cacheService.SetAsync(cacheKey, entities, tags: SupportingDocsConstants.ListTags);
        
        _logger.LogDebug("Retrieved {Count} supporting documents for user {UserId}", entities.Count, userId);
        return mapper.Map<List<SupportingDocModel>>(entities);
    }
}