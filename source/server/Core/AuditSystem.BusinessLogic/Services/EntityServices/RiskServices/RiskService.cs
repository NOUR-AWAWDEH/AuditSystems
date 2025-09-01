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
    ILogger<RiskService> logger,
    IRiskValidation validationHelper,
    IRiskSearch searchHelper) : 

    UnifiedCacheService<Guid, Risk, RiskModel>(
        repository,
        mapper,
        cacheService,
        logger,
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

        logger.LogInformation("Creating risk {RiskName} for user {UserId}", riskModel.RiskName, userId);

        await _validationHelper.ValidateObjectiveOwnershipAsync(riskModel.ObjectiveId, userId);

        var entity = mapper.Map<Risk>(riskModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created risk {RiskId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<RiskModel> GetRiskByIdAsync(Guid id, Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving risk {RiskId} for user {UserId}", id, userId);
        
        var cachedResult = await TryGetFromCacheWithValidationAsync<RiskModel>(
            id,
            userId,
            entity => entity.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
            RiskConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved risk {RiskId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateRiskOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<RiskModel>(entity);
    }

    public async Task DeleteRiskAsync(Guid riskId, Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Deleting risk {RiskId} for user {UserId}", riskId, userId);

        await _validationHelper.ValidateRiskOwnershipAsync(riskId, userId);
        
        await repository.DeleteAsync(riskId);

        await ClearEntityCacheAsync(riskId);
        
        await ClearUserRelatedCachesAsync(userId, RiskConstants.EntityName);

        logger.LogInformation("Successfully deleted risk {RiskId}", riskId);
    }
    
    public async Task UpdateRiskAsync(RiskModel riskModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskModel, nameof(riskModel));
        RiskValidationHelper.ValidateUserId(userId);
    
        logger.LogInformation("Updating risk {RiskId} for user {UserId}", riskModel.Id, userId);
    
        var existingEntity = await _validationHelper.ValidateRiskOwnershipAsync(riskModel.Id, userId);
        
        await _validationHelper.ValidateObjectiveOwnershipAsync(riskModel.ObjectiveId, userId);
        
        mapper.Map(riskModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
    
        logger.LogInformation("Successfully updated risk {RiskId}", riskModel.Id);
    }
    
    public async Task<IReadOnlyList<RiskModel>> GetRisksByUserIdAsync(Guid userId)
    {
        RiskValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all risks for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(ap => new RiskModel
            {
                Id = ap.Id,
                RiskName = ap.RiskName,
                ObjectiveId = ap.ObjectiveId,
                RateId = ap.RateId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt,
            })
            .ToListAsync();
    }

    public async Task<PaginationResult<RiskModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid objectiveId, Page? page)
    {
        RiskValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching risks for user {UserId} with term: {SearchTerm} and objective: {ObjectiveId}",
            userId, searchTerm ?? "(none)", objectiveId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, objectiveId, page);

        var cachedResult = await GetFromCacheAsync<RiskModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(RiskConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, objectiveId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskModel>> SearchAsync(string? name, Page page) 
    { 
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching risks with name filter: {RiskName}", name ?? "(none)");

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