using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.BusinessLogic.Helpers.RiskHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskService(
    IRepository<Guid, Risk> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskService> _logger,
    IRiskValidation validationHelper,
    IRiskSearch searchHelper) : 

    UnifiedCacheService<Guid, Risk, RiskModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Risk,
        RiskConstants.RiskTags,
        RiskConstants.ListTags
    ),
    IRiskService
{
    private readonly IRiskValidation _validationHelper = validationHelper;
    private readonly IRiskSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateRiskAsync(RiskModel riskModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));
        RiskValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating risk {RiskName} for user {UserId}", riskModel.RiskName, userId);

        await _validationHelper.ValidateObjectiveOwnershipAsync(riskModel.ObjectiveId, userId);

        var entity = mapper.Map<Risk>(riskModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created risk {RiskId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<RiskModel> GetRiskByIdAsync(Guid id, Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving risk {RiskId} for user {UserId}", id, userId);
        
        var cachedResult = await TryGetFromCacheWithValidationAsync<RiskModel>(
            id,
            userId,
            RiskConstants.RiskOwnerIdSelector,
            RiskConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved risk {RiskId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateRiskOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<RiskModel>(entity);
    }

    public async Task DeleteRiskAsync(Guid riskId, Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);
        
        _logger.LogDebug("Deleting risk {RiskId} for user {UserId}", riskId, userId);

        await _validationHelper.ValidateRiskOwnershipAsync(riskId, userId);
    
        var risk = await repository.GetByIdAsync(riskId);
        Guid? objectiveId = risk?.ObjectiveId;

        await repository.DeleteAsync(riskId);
        await ClearEntityCacheAsync(riskId);
        await ClearUserRelatedCachesAsync(userId, RiskConstants.EntityName, objectiveId);

        _logger.LogInformation("Successfully deleted risk {RiskId} for user {UserId}", riskId, userId);
    }
    
    public async Task UpdateRiskAsync(RiskModel riskModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));
        RiskValidationHelper.ValidateUserId(userId);
    
        _logger.LogInformation("Updating risk {RiskId} for user {UserId}", riskModel.Id, userId);
    
        var existingEntity = await _validationHelper.ValidateRiskOwnershipAsync(riskModel.Id, userId);
        
        await _validationHelper.ValidateObjectiveOwnershipAsync(riskModel.ObjectiveId, userId);
        
        mapper.Map(riskModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
    
        _logger.LogInformation("Successfully updated risk {RiskId}", riskModel.Id);
    }
    
    public async Task<IReadOnlyList<RiskModel>> GetRisksByUserIdAsync(Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all risks for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(RiskConstants.RiskOwnerPredicate(userId))
            .Select(ap => new RiskModel
            {
                Id = ap.Id,
                RiskName = ap.RiskName,
                ObjectiveId = ap.ObjectiveId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt,
            })
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<RiskModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid objectiveId, Page? page)
    {
        RiskValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching risks for user {UserId} with term: {SearchTerm} and objective: {ObjectiveId}",
            userId, searchTerm ?? "(none)", objectiveId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, objectiveId, page);

        var cachedResult = await GetFromCacheAsync<RiskModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(RiskConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, objectiveId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskModel>> SearchAsync(string? name, Page page) 
    { 
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching risks with name filter: {RiskName}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RiskModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(RiskConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<RiskModel>(cacheKey, countCacheKey, page, name);
    }
}