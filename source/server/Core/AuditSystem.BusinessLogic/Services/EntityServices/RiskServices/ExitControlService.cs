using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class ExitControlService(
    IRepository<Guid, ExitControl> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ExitControlService> _logger,
    IExitControlValidation validationHelper,
    IExitControlSearch searchHelper) : 

    UnifiedCacheService<Guid, ExitControl, ExitControlModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Risk,
        ExitControlConstants.ExitControlTags,
        ExitControlConstants.ListTags
    ),
    IExitControlService
{
    private readonly IExitControlValidation _validationHelper = validationHelper;
    private readonly IExitControlSearch _searchHelper = searchHelper;  
    
    public async Task<Guid> CreateExitControlAsync(ExitControlModel exitControlModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(exitControlModel, nameof(exitControlModel));
        ExitControlValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating exit Control {Description} for user {UserId}", exitControlModel.Description, userId);

        await _validationHelper.ValidateObjectiveOwnershipAsync(exitControlModel.ObjectiveId, userId);

        var entity = mapper.Map<ExitControl>(exitControlModel);
        entity.CreatedByUserId = userId;
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created exit Control {ExitControlId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<ExitControlModel> GetExitControlByIdAsync(Guid id, Guid userId)
    {
        ExitControlValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving exit Control {ExitControlId} for user {UserId}", id, userId);
        
        var cachedResult = await TryGetFromCacheWithValidationAsync<ExitControlModel>(
            id,
            userId,
            ExitControlConstants.ExitControlOwnerIdSelector,
            ExitControlConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved exit Control {ExitControlId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateExitControlOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<ExitControlModel>(entity);
    }

    public async Task DeleteExitControlAsync(Guid exitControlId, Guid userId)
    {
        ExitControlValidationHelper.ValidateUserId(userId);
        
        _logger.LogDebug("Deleting exit Control {ExitControlId} for user {UserId}", exitControlId, userId);

        await _validationHelper.ValidateExitControlOwnershipAsync(exitControlId, userId);
        
        var exitControl = await repository.GetByIdAsync(exitControlId);
        Guid? objectiveId = exitControl?.ObjectiveId;

        await repository.DeleteAsync(exitControlId);
        await ClearEntityCacheAsync(exitControlId);
        await ClearUserRelatedCachesAsync(userId, ExitControlConstants.EntityName, objectiveId);

        _logger.LogInformation("Successfully deleted exit Control {ExitControlId} for user {UserId}", exitControlId, userId);
    }
    
    public async Task UpdateExitControlAsync(ExitControlModel exitControlModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(exitControlModel, nameof(exitControlModel));
        ExitControlValidationHelper.ValidateUserId(userId);
    
        _logger.LogInformation("Updating exit Control {ExitControlId} for user {UserId}", exitControlModel.Id, userId);
    
        var existingEntity = await _validationHelper.ValidateExitControlOwnershipAsync(exitControlModel.Id, userId);
        
        await _validationHelper.ValidateObjectiveOwnershipAsync(exitControlModel.ObjectiveId, userId);
        
        mapper.Map(exitControlModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
    
        _logger.LogInformation("Successfully updated exit Control {ExitControlId}", exitControlModel.Id);
    }
    
    public async Task<IReadOnlyList<ExitControlModel>> GetExitControlsByUserIdAsync(Guid userId)
    {
        ExitControlValidationHelper.ValidateUserId(userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(ExitControlConstants.ExitControlOwnerPredicate(userId))
            .Select(ec => new ExitControlModel
            {
                Id = ec.Id,
                Description = ec.Description,
                Inadequate = ec.Inadequate,
                ObjectiveId = ec.ObjectiveId,
                CreatedAt = ec.CreatedAt,
                UpdatedAt = ec.UpdatedAt,
            })
            .OrderBy(ec => ec.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<ExitControlModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid observationId, Page? page)
    {
        ExitControlValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching exit controls for user {UserId} with term: {SearchTerm} and observation: {ObservationId}",
            userId, searchTerm ?? "(none)", observationId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, observationId, page);

        var cachedResult = await GetFromCacheAsync<ExitControlModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(ExitControlConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, observationId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ExitControlModel>> SearchAsync(string? name, Page page) 
    { 
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching exit controls with name filter: {ExitControlName}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<ExitControlModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(ExitControlConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<ExitControlModel>(cacheKey, countCacheKey, page, name);
    }
}