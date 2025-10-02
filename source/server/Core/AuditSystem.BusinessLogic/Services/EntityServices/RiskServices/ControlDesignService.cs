using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.ControlDesignConst;
using AuditSystem.BusinessLogic.Helpers.ControlDesignHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;


internal sealed class ControlDesignService(
    IRepository<Guid, ControlDesign> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<ControlDesignService> _logger,
    IControlDesignValidation validationHelper,
    IControlDesignSearch searchHelper) :
    UnifiedCacheService<Guid, ControlDesign, ControlDesignModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.ControlDesign,
        ControlDesignConstants.ControlDesignTags,
        ControlDesignConstants.ListTags
    ),
    IControlDesignService
{
    private readonly IControlDesignValidation _validationHelper = validationHelper;
    private readonly IControlDesignSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateControlDesignAsync(ControlDesignModel controlDesignModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(controlDesignModel, nameof(controlDesignModel));
        ControlDesignValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating  control design {ControlDesignDescription} for user {UserId}", controlDesignModel.Description, userId);

        await _validationHelper.ValidateExitControlOwnershipAsync(controlDesignModel.ExitControlId, userId);

        var entity = mapper.Map<ControlDesign>(controlDesignModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully created  control design {ControlDesignId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task DeleteControlDesignAsync(Guid controlDesignId, Guid userId)
    {
        ControlDesignValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Deleting  control design {ControlDesignId} for user {UserId}", controlDesignId, userId);

        await _validationHelper.ValidateControlDesignOwnershipAsync(controlDesignId, userId);

        var ControlDesign = await repository.GetByIdAsync(controlDesignId);
        var exitControlId = ControlDesign?.ExitControlId;

        await repository.DeleteAsync(controlDesignId);
        await ClearEntityCacheAsync(controlDesignId);

        await ClearUserRelatedCachesAsync(userId, ControlDesignConstants.EntityName, exitControlId);

        _logger.LogInformation("Successfully deleted  control design {ControlDesignId}", controlDesignId);
    }

    public async Task<ControlDesignModel> GetControlDesignByIdAsync(Guid id, Guid userId)
    {
        ControlDesignValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving  control design {ControlDesignId} for user {UserId}", id, userId);

        var cashedResult = await TryGetFromCacheWithValidationAsync<ControlDesignModel>(
            id,
            userId,
            entity => entity.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
            ControlDesignConstants.UnauthorizedAccessMessage);

        if (cashedResult != null)
        {
            logger.LogDebug("Retrieved  control design {ControlDesignId} from cache", id);
            return cashedResult;
        }

        var entity = await _validationHelper.ValidateControlDesignOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<ControlDesignModel>(entity);
    }

    public async Task<IReadOnlyList<ControlDesignModel>> GetControlDesignsByUserIdAsync(Guid userId)
    {
        ControlDesignValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all  control designs for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Include(s => s.ExitControl)
            .Where(r => r.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(d => new ControlDesignModel
            {
                Id = d.Id,
                Description = d.Description,
                ExitControlId = d.ExitControlId,
                Inadequate = d.Inadequate,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(cd => cd.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<ControlDesignModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searcing control design whith name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<ControlDesignModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(ControlDesignConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<ControlDesignModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<ControlDesignModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid exitControlId, Page? page)
    {
        ControlDesignValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching  control designs for user {UserId} with term: {SearchTerm} and exit control: {ExitControlId}",
            userId, searchTerm ?? "(none)", exitControlId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, exitControlId, page);

        var cachedResult = await GetFromCacheAsync<ControlDesignModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(ControlDesignConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, exitControlId, page, cacheKey, countCacheKey);
        return result;
    }

    public async Task UpdateControlDesignAsync(ControlDesignModel ControlDesignModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(ControlDesignModel, nameof(ControlDesignModel));
        ControlDesignValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating  control design {ControlDesignId} for user {UserId}", ControlDesignModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateControlDesignOwnershipAsync(ControlDesignModel.Id, userId);

        mapper.Map(ControlDesignModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully updated  control design {ControlDesignId}", ControlDesignModel.Id);
    }
}