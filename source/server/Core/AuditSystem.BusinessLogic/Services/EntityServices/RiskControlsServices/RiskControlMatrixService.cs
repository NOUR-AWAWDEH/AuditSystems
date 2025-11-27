using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlMatrixService(
    IRepository<Guid, RiskControlMatrix> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RiskControlMatrixService> _logger,
    IRiskControlMatrixValidation validationHelper,
    IRiskControlMatrixSearch searchHelper) : 
    UnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.RiskControlMatrix,
        RiskControlMatrixConstants.RiskControlMatrixTags,
        RiskControlMatrixConstants.ListTags
    ),
    IRiskControlMatrixService
{
    private readonly IRiskControlMatrixValidation _validationHelper = validationHelper;
    private readonly IRiskControlMatrixSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Creating risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixModel.Id, userId);

        await _validationHelper.ValidateSubProcessOwnershipAsync(riskControlMatrixModel.SubProcessId, userId);

        var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
        entity.CreatedByUserId = userId;
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogDebug("Successfully created risk control matrix {RiskControlMatrixId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }
    
    public async Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id, Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving risk control matrix {RiskControlMatrixId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<RiskControlMatrixModel>(
            id,
            userId,
            RiskControlMatrixConstants.RiskControlMatrixOwnerIdSelector,
            RiskControlMatrixConstants.UnauthorizedAccessMessage);

        if (cachedResult != null) 
        {
            _logger.LogDebug("Retrieved risk control matrix {RiskControlMatrixId} from cache for user {UserId}", id, userId);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<RiskControlMatrixModel>(entity);
    }

    public async Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId, Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);
        _logger.LogDebug("Deleting risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixId, userId);
        
        await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(riskControlMatrixId, userId);
        
        // Get the risk control matrix first to retrieve the SubProcessId before deletion
        var riskControlMatrix = await repository.GetByIdAsync(riskControlMatrixId);
        Guid? subProcessId = riskControlMatrix?.SubProcessId;

        await repository.DeleteAsync(riskControlMatrixId);
        await ClearEntityCacheAsync(riskControlMatrixId);
        await ClearUserRelatedCachesAsync(userId, RiskControlMatrixConstants.EntityName, subProcessId);

        _logger.LogInformation("Successfully deleted risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixId, userId);
    }

    public async Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Updating risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(riskControlMatrixModel.Id, userId);
        await _validationHelper.ValidateSubProcessOwnershipAsync(riskControlMatrixModel.SubProcessId, userId);

        mapper.Map(riskControlMatrixModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated risk control matrix {RiskControlMatrixId} for user {UserId}", existingEntity.Id, userId);
    }

    public async Task<IReadOnlyList<RiskControlMatrixModel>> GetRiskControlMatricesByUserIdAsync(Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all risk control matrices for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(RiskControlMatrixConstants.RiskControlMatrixOwnerPredicate(userId))
            .Select(rcm => new RiskControlMatrixModel
            {
                Id = rcm.Id,
                Description = rcm.Description,
                SubProcessId = rcm.SubProcessId,
                CreatedAt = rcm.CreatedAt,
                UpdatedAt = rcm.UpdatedAt,
            })
            .OrderBy(rcm => rcm.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid subProcessId, Page? page)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching risk control matrices for user {UserId} with term: {SearchTerm} and sub process: {SubProcessId}",
                userId, searchTerm ?? "(none)", subProcessId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, subProcessId, page);

        var cachedResult = await GetFromCacheAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(RiskControlMatrixConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, subProcessId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? description, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching risk control matrices with Description filter: {Description}", description?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(description, page);

        var cachedResult = await GetFromCacheAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        { 
            _logger.LogDebug(RiskControlMatrixConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page, description);
    }
}