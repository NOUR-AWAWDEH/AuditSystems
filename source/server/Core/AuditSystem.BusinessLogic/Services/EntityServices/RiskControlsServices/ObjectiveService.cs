using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal class ObjectiveService(
    IRepository<Guid, Objective> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ObjectiveService> logger,
    IObjectiveValidation validationHelper,
    IObjectiveSearch searchHelper) :
    UnifiedCacheService<Guid, Objective, ObjectiveModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.Objective,
        ObjectiveConstants.ObjectiveTags,
        ObjectiveConstants.ListTags
    ),
    IObjectiveService
{
    private readonly IObjectiveValidation _validationHelper = validationHelper;
    private readonly IObjectiveSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateObjectiveAsync(ObjectiveModel objectiveModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(ObjectiveModel));
        ObjectiveValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Creating objective {ObjectiveId} for user {UserId}", objectiveModel.Id, userId);

        await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(objectiveModel.RiskControlMatrixId, userId);

        var entity = mapper.Map<Objective>(objectiveModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogDebug("Successfully created objective {ObjectiveId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }
    
    public async Task<ObjectiveModel> GetObjectiveByIdAsync(Guid id, Guid userId)
    {
        ObjectiveValidationHelper.ValidateUserId(userId);
        
        logger.LogDebug("Retrieveing objective {ObjectiveId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<ObjectiveModel>(
            id,
            userId,
            entity => entity.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
            ObjectiveConstants.UnauthorizedAccessMessage);

        if (cachedResult != null) 
        { 
            logger.LogDebug("Retrive objective {ObjectiveId} from cache for user {UserId}", id, userId);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateObjectiveOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<ObjectiveModel>(entity);
    }
    
    public async Task DeleteObjectiveAsync(Guid objectiveId, Guid userId)
    {
        ObjectiveValidationHelper.ValidateUserId(userId);
        logger.LogDebug("Deleting objective {ObjectiveId} for user {UserId}", objectiveId, userId);
        
        await _validationHelper.ValidateObjectiveOwnershipAsync(objectiveId, userId);
        await repository.DeleteAsync(objectiveId);
        await ClearEntityCacheAsync(objectiveId);
        await ClearUserRelatedCachesAsync(userId, ObjectiveConstants.EntityName);
        
        logger.LogInformation("Successfully deleted objective {ObjectiveId} for user {UserId}", objectiveId, userId);
    }
    
    public async Task UpdateObjectiveAsync(ObjectiveModel objectiveModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(objectiveModel, nameof(objectiveModel));
        ObjectiveValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Updating objective {ObjectiveId} for user {UserId}", objectiveModel.Id, userId);
        
        var existingEntity = await _validationHelper.ValidateObjectiveOwnershipAsync(objectiveModel.Id, userId);
        await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(objectiveModel.RiskControlMatrixId, userId);

        mapper.Map(objectiveModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated objective {ObjectiveId} for user {UserId}", existingEntity.Id, userId);
    }

    public async Task<ICollection<ObjectiveModel>> GetObjectivesByUserIdAsync(Guid userId)
    {
        ObjectiveValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all objectives for user {UserId}", userId);

        var query = repository.GetAll()
            .Include(ob => ob.RiskControlMatrix)
            .ThenInclude(ob => ob.SubProcess)
            .ThenInclude(ob => ob.AuditProcess)
            .ThenInclude(ob => ob.SubDepartment)
            .ThenInclude(ob => ob.Department)
            .ThenInclude(ob => ob.SubLocation)
            .ThenInclude(ob => ob.Location)
            .Where(ob => ob.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(ob => new ObjectiveModel
            {
                Id = ob.Id,
                Description = ob.Description,
                RiskControlMatrixId = ob.RiskControlMatrixId,
                CreatedAt = ob.CreatedAt,
                UpdatedAt = ob.UpdatedAt,
            });

        var objectives = await query.ToListAsync();
        logger.LogDebug("Retrieved {ObjectivesCount} objectives for user {UserId}", objectives.Count, userId);

        return objectives;
    }

    public async Task<PaginationResult<ObjectiveModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid riskControlMatrixId, Page? page)
    {
        ObjectiveValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching objectives for user {UserId} with term: {SearchTerm} and risk control matrix: {RiskControlMatrixId}",
                userId, searchTerm ?? "(none)", riskControlMatrixId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, riskControlMatrixId, page);

        var cachedResult = await GetFromCacheAsync<ObjectiveModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(ObjectiveConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, riskControlMatrixId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ObjectiveModel>> SearchAsync(string? description, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(description, page);

        var cachedResult = await GetFromCacheAsync<ObjectiveModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            logger.LogDebug(ObjectiveConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }
        

        return await GetFromRepositoryAsync<ObjectiveModel>(cacheKey, countCacheKey, page, description);
    }
}