using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.BusinessLogic.Helpers.ObservationHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Observations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ObservationServices;

internal sealed class ObservationService(
    IRepository<Guid, Observation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ObservationService> _logger,
    IObservationValidation validationHelper,
    IObservationSearch searchHelper) : 
    UnifiedCacheService<Guid, Observation, ObservationModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Observation,
        ObservationConstants.ObservationTags,
        ObservationConstants.ListTags
    ),
    IObservationService
{
    private readonly IObservationValidation _validationHelper = validationHelper;
    private readonly IObservationSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateObservationAsync(ObservationModel observationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(observationModel, nameof(observationModel));
        ObservationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating observation {ObservationName} for user {UserId}", observationModel.Description, userId);

        await _validationHelper.ValidateAuditProgramOwnershipAsync(observationModel.AuditProgramId, userId);

        var entity = mapper.Map<Observation>(observationModel);
        entity.CreatedByUserId = userId;
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity); 
        await InvalidateEntityTagsCacheAsync();

        logger.LogInformation("Successfully created observation {ObservationId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task DeleteObservationAsync(Guid observationId, Guid userId)
    {
        ObservationValidationHelper.ValidateUserId(userId);
            
        logger.LogInformation("Deleting observation {ObservationId} for user {UserId}", observationId, userId);

        await _validationHelper.ValidateObservationOwnershipAsync(observationId, userId);

        var observation = await repository.GetByIdAsync(observationId);
        var auditProgramId = observation?.AuditProgramId;

        await repository.DeleteAsync(observationId);
        await ClearEntityCacheAsync(observationId);
        
        await ClearUserRelatedCachesAsync(userId, ObservationConstants.EntityName, auditProgramId);

        logger.LogInformation("Successfully deleted observation {ObservationId}", observationId);
    }

    public async Task<ObservationModel> GetObservationByIdAsync(Guid id, Guid userId)
    {
        ObservationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving observation {ObservationId} for user {UserId}",id,userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<ObservationModel>(
            id,
            userId,
            ObservationConstants.ObservationOwnerIdSelector,
            ObservationConstants.UnauthorizedAuditProgramAccessMessage);

        if (cashedResult != null) 
        {   
            logger.LogDebug("Retrieved observation {ObservationId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateObservationOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<ObservationModel>(entity);
    }

    public async Task<IReadOnlyList<ObservationModel>> GetObservationsByUserIdAsync(Guid userId)
    {
        ObservationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all observations for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Where(ObservationConstants.ObservationOwnerPredicate(userId))
            .Select(d => new ObservationModel
            {
                Id = d.Id,
                Description = d.Description,
                ActionOwner = d.ActionOwner,
                Background = d.Background,
                ActionPlan = d.ActionPlan,
                ActionDate = d.ActionDate,
                Note = d.Note,
                AuditProgramId = d.AuditProgramId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<ObservationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searcing observations whith name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<ObservationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(ObservationConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<ObservationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<ObservationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid auditProgramId, Page? page)
    {
        ObservationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching observations for user {UserId} with term: {SearchTerm} and audit program: {AuditProgramId}",
            userId, searchTerm ?? "(none)", auditProgramId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, auditProgramId, page);

        var cachedResult = await GetFromCacheAsync<ObservationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(ObservationConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, auditProgramId, page, cacheKey, countCacheKey);
        return result;
    }

    public async Task UpdateObservationAsync(ObservationModel observationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(observationModel, nameof(observationModel));
        ObservationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating observation {ObservationId} for user {UserId}", observationModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateObservationOwnershipAsync(observationModel.Id, userId);
        
        mapper.Map(observationModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully updated observation {ObservationId}", observationModel.Id);
    }
}