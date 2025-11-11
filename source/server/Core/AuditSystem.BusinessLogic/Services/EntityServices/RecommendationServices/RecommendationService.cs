namespace AuditSystem.BusinessLogic.Services.EntityServices.RecommendationServices;

using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.Application.Constants.RecommendationConst;
using AuditSystem.BusinessLogic.Helpers.ObservationHelpers;
using AuditSystem.BusinessLogic.Helpers.RecommendationHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Recommendations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

internal sealed class RecommendationService(
    IRepository<Guid, Recommendation> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RecommendationService> _logger,
    IRecommendationValidation validationHelper,
    IRecommendationSearch searchHelper) : 
    UnifiedCacheService<Guid, Recommendation, RecommendationModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Recommendation,
        RecommendationConstants.RecommendationTags,
        RecommendationConstants.ListTags
    ),
    IRecommendationService
{
    private readonly IRecommendationValidation _validationHelper = validationHelper;
    private readonly IRecommendationSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateRecommendationAsync(RecommendationModel recommendationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(recommendationModel, nameof(recommendationModel));
        RecommendationValidationHelper.ValidateUserId(userId);
        
        _logger.LogInformation("Creating recommendation {RecommendationName} for user {UserId}", recommendationModel.Title, userId);

        await _validationHelper.ValidateObservationOwnershipAsync(recommendationModel.ObservationId, userId);

        var entity = mapper.Map<Recommendation>(recommendationModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity); 
        await InvalidateEntityTagsCacheAsync();

        logger.LogInformation("Successfully created recommendation {RecommendationId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task DeleteRecommendationAsync(Guid recommendationId, Guid userId)
    {
        RecommendationValidationHelper.ValidateUserId(userId);
            
        logger.LogInformation("Deleting recommendation {RecommendationId} for user {UserId}", recommendationId, userId);

        await _validationHelper.ValidateRecommendationOwnershipAsync(recommendationId, userId);

        var recommendation = await repository.GetByIdAsync(recommendationId);
        var observationId = recommendation?.ObservationId;

        await repository.DeleteAsync(recommendationId);
        await ClearEntityCacheAsync(recommendationId);
        
        await ClearUserRelatedCachesAsync(userId, RecommendationConstants.EntityName, observationId);

        logger.LogInformation("Successfully deleted recommendation {RecommendationId}", recommendationId);
    }

    public async Task<RecommendationModel> GetRecommendationByIdAsync(Guid id, Guid userId)
    {
        RecommendationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving recommendation {RecommendationId} for user {UserId}",id,userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<RecommendationModel>(
            id,
            userId,
            RecommendationConstants.RecommendationOwnerIdSelector,
            RecommendationConstants.UnauthorizedObservationAccessMessage);

        if (cashedResult != null) 
        {   
            logger.LogDebug("Retrieved recommendation {RecommendationId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateRecommendationOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<RecommendationModel>(entity);
    }

    public async Task<IReadOnlyList<RecommendationModel>> GetRecommendationsByUserIdAsync(Guid userId)
    {
        RecommendationValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all recommendations for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Where(RecommendationConstants.RecommendationOwnerPredicate(userId))
            .Select(d => new RecommendationModel
            {
                Id = d.Id,
                Description = d.Description,
                Title = d.Title,
                ObservationId = d.ObservationId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<RecommendationModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searcing recommendations whith name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<RecommendationModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(ObservationConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<RecommendationModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<RecommendationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid observationId, Page? page)
    {
        RecommendationValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching recommendations for user {UserId} with term: {SearchTerm} and observation: {ObservationId}",
            userId, searchTerm ?? "(none)", observationId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, observationId, page);

        var cachedResult = await GetFromCacheAsync<RecommendationModel>(cacheKey, countCacheKey, page);
        
        if (cachedResult != null) 
        {
            _logger.LogDebug(RecommendationConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        
        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, observationId, page, cacheKey, countCacheKey);
        return result;
    }

    public async Task UpdateRecommendationAsync(RecommendationModel recommendationModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(recommendationModel, nameof(recommendationModel));
        RecommendationValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating recommendation {RecommendationId} for user {UserId}", recommendationModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateRecommendationOwnershipAsync(recommendationModel.Id, userId);
        
        mapper.Map(recommendationModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully updated recommendation {RecommendationId}", recommendationModel.Id);
    }
}