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
    ILogger<RiskControlMatrixService> logger,
    IRiskControlMatrixValidation validationHelper,
    IRiskControlMatrixSearch searchHelper) : 
    UnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>(
        repository,
        mapper,
        cacheService,
        logger,
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

        logger.LogDebug("Creating risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixModel.Id, userId);

        await _validationHelper.ValidateSubProcessOwnershipAsync(riskControlMatrixModel.SubProcessId, userId);

        var entity = mapper.Map<RiskControlMatrix>(riskControlMatrixModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogDebug("Successfully created risk control matrix {RiskControlMatrixId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }
    
    public async Task<RiskControlMatrixModel> GetRiskControlMatrixByIdAsync(Guid id, Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving risk control matrix {RiskControlMatrixId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<RiskControlMatrixModel>(
            id,
            userId,
            entity => entity.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
            RiskControlMatrixConstants.UnauthorizedAccessMessage);

        if (cachedResult != null) 
        {
            logger.LogDebug("Retrieved risk control matrix {RiskControlMatrixId} from cache for user {UserId}", id, userId);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<RiskControlMatrixModel>(entity);
    }

    public async Task DeleteRiskControlMatrixAsync(Guid riskControlMatrixId, Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);
        logger.LogDebug("Deleting risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixId, userId);
        
        await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(riskControlMatrixId, userId);
        await repository.DeleteAsync(riskControlMatrixId);
        await ClearEntityCacheAsync(riskControlMatrixId);
        await ClearUserRelatedCachesAsync(userId, RiskControlMatrixConstants.EntityName);

        logger.LogInformation("Successfully deleted risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixId, userId);
    }

    public async Task UpdateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(riskControlMatrixModel, nameof(riskControlMatrixModel));
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Updating risk control matrix {RiskControlMatrixId} for user {UserId}", riskControlMatrixModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateRiskControlMatrixOwnershipAsync(riskControlMatrixModel.Id, userId);
        await _validationHelper.ValidateSubProcessOwnershipAsync(riskControlMatrixModel.SubProcessId, userId);

        mapper.Map(riskControlMatrixModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated risk control matrix {RiskControlMatrixId} for user {UserId}", existingEntity.Id, userId);
    }

    public async Task<ICollection<RiskControlMatrixModel>> GetRiskControlMatricesByUserIdAsync(Guid userId)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all risk control matrices for user {UserId}", userId);

        var query = repository.GetAll()
            .Include(rcm => rcm.SubProcess)
            .ThenInclude(rcm => rcm.AuditProcess)
            .ThenInclude(rcm => rcm.SubDepartment)
            .ThenInclude(rcm => rcm.Department)
            .ThenInclude(rcm => rcm.SubLocation)
            .ThenInclude(rcm => rcm.Location)
            .Where(rcm => rcm.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(rcm => new RiskControlMatrixModel
            {
                Id = rcm.Id,
                Description = rcm.Description,
                SubProcessId = rcm.SubProcessId,
                CreatedAt = rcm.CreatedAt,
                UpdatedAt = rcm.UpdatedAt,
            });

        var riskControlMatrices = await query.ToListAsync();
        logger.LogDebug("Retrieved {RiskControlMatricesCount} risk control matrices for user {UserId}", riskControlMatrices.Count, userId);

        return riskControlMatrices;
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid subProcessId, Page? page)
    {
        RiskControlMatrixValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching risk control matrices for user {UserId} with term: {SearchTerm} and sub process: {SubProcessId}",
                userId, searchTerm ?? "(none)", subProcessId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, subProcessId, page);

        var cachedResult = await GetFromCacheAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(RiskControlMatrixConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, subProcessId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> SearchAsync(string? description, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching risk control matrices with Description filter: {Description}", description?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(description, page);

        var cachedResult = await GetFromCacheAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        { 
            logger.LogDebug(RiskControlMatrixConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<RiskControlMatrixModel>(cacheKey, countCacheKey, page, description);
    }
}