using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditProgramService(
    IRepository<Guid, AuditProgram> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditProgramService> _logger,
    IAuditProgramValidation validationHelper,
    IAuditProgramSearch searchHelper) : 
    UnifiedCacheService<Guid, AuditProgram, AuditProgramModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.AuditProgram,
        AuditProgramConstants.AuditProgramTags,
        AuditProgramConstants.ListTags
    ),
    IAuditProgramService
{
    private readonly IAuditProgramValidation _validationHelper = validationHelper;
    private readonly IAuditProgramSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(auditProgramModel, nameof(auditProgramModel));
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating audit program {AuditProgramName} for user {UserId}", auditProgramModel.Description, userId);

        await _validationHelper.ValidateExitControlOwnershipAsync(auditProgramModel.ExitControlId, userId);

        var entity = mapper.Map<AuditProgram>(auditProgramModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity); 
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully created audit program {AuditProgramId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task DeleteAuditProgramAsync(Guid auditProgramId, Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);
            
        _logger.LogInformation("Deleting audit program {AuditProgramId} for user {UserId}", auditProgramId, userId);

        await _validationHelper.ValidateAuditProgramOwnershipAsync(auditProgramId, userId);

        var auditProgram = await repository.GetByIdAsync(auditProgramId);
        var exitControlId = auditProgram?.ExitControlId;

        await repository.DeleteAsync(auditProgramId);
        await ClearEntityCacheAsync(auditProgramId);
        
        // Clear all related caches
        await ClearUserRelatedCachesAsync(userId, AuditProgramConstants.EntityName, exitControlId);
        await InvalidateEntityTagsCacheAsync();
        
        _logger.LogInformation("Successfully deleted audit program {AuditProgramId}", auditProgramId);
    }

    public async Task<AuditProgramModel> GetAuditProgramByIdAsync(Guid id, Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving audit program {AuditProgramId} for user {UserId}", id, userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<AuditProgramModel>(
            id,
            userId,
            AuditProgramConstants.AuditProgramOwnerIdSelector,
            AuditProgramConstants.UnauthorizedAccessMessage);

        if (cashedResult != null) 
        {
            _logger.LogDebug("Retrieved audit program {AuditProgramId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateAuditProgramOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<AuditProgramModel>(entity);
    }

    public async Task<IReadOnlyList<AuditProgramModel>> GetAuditProgramsByUserIdAsync(Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all audit programs for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Where(AuditProgramConstants.AuditProgramOwnerPredicate(userId))
            .Select(d => new AuditProgramModel
            {
                Id = d.Id,
                Description = d.Description,
                ExitControlId = d.ExitControlId,
                ControlDesignId = d.ControlDesignId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(ap => ap.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<AuditProgramModel>> SearchAsync(string? name, Page? page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searcing audit programs whith name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditProgramModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(AuditProgramConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<AuditProgramModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<AuditProgramModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid exitControlId, Page? page)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching audit programs for user {UserId} with term: {SearchTerm} and exit control: {ExitControlId}",
            userId, searchTerm ?? "(none)", exitControlId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, exitControlId, page);

        var cachedResult = await GetFromCacheAsync<AuditProgramModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(AuditProgramConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, exitControlId, page, cacheKey, countCacheKey);
        return result;
    }

    public async Task UpdateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(auditProgramModel, nameof(auditProgramModel));
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating audit program {AuditProgramId} for user {UserId}", auditProgramModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateAuditProgramOwnershipAsync(auditProgramModel.Id, userId);
        
        mapper.Map(auditProgramModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully updated audit program {AuditProgramId}", auditProgramModel.Id);
    }
}