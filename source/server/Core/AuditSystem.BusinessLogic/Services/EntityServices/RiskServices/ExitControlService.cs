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
    ILogger<ExitControlService> logger,
    IExitControlValidation validationHelper,
    IExitControlSearch searchHelper) : 

    UnifiedCacheService<Guid, ExitControl, ExitControlModel>(
        repository,
        mapper,
        cacheService,
        logger,
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

        logger.LogInformation("Creating exit Control {Description} for user {UserId}", exitControlModel.Description, userId);

        await _validationHelper.ValidateRiskOwnershipAsync(exitControlModel.RiskId, userId);

        var entity = mapper.Map<ExitControl>(exitControlModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created exit Control {ExitControlId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<ExitControlModel> GetExitControlByIdAsync(Guid id, Guid userId)
    {
        ExitControlValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving exit Control {ExitControlId} for user {UserId}", id, userId);
        
        var cachedResult = await TryGetFromCacheWithValidationAsync<ExitControlModel>(
            id,
            userId,
            entity => entity.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
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
        
        logger.LogDebug("Deleting exit Control {ExitControlId} for user {UserId}", exitControlId, userId);

        await _validationHelper.ValidateExitControlOwnershipAsync(exitControlId, userId);
        
        var exitControl = await repository.GetByIdAsync(exitControlId);
        Guid? riskId = exitControl?.RiskId;

        await repository.DeleteAsync(exitControlId);
        await ClearEntityCacheAsync(exitControlId);
        await ClearUserRelatedCachesAsync(userId, ExitControlConstants.EntityName, riskId);

        logger.LogInformation("Successfully deleted exit Control {ExitControlId} for user {UserId}", exitControlId, userId);
    }
    
    public async Task UpdateExitControlAsync(ExitControlModel exitControlModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(exitControlModel, nameof(exitControlModel));
        ExitControlValidationHelper.ValidateUserId(userId);
    
        logger.LogInformation("Updating exit Control {ExitControlId} for user {UserId}", exitControlModel.Id, userId);
    
        var existingEntity = await _validationHelper.ValidateExitControlOwnershipAsync(exitControlModel.Id, userId);
        
        await _validationHelper.ValidateRiskOwnershipAsync(exitControlModel.RiskId, userId);
        
        mapper.Map(exitControlModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(existingEntity);
    
        await CacheEntityAsync(existingEntity);
    
        logger.LogInformation("Successfully updated exit Control {ExitControlId}", exitControlModel.Id);
    }
    
    public async Task<IReadOnlyList<ExitControlModel>> GetExitControlsByUserIdAsync(Guid userId)
    {
        ExitControlValidationHelper.ValidateUserId(userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(r => r.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess
                .SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(ec => new ExitControlModel
            {
                Id = ec.Id,
                Description = ec.Description,
                Inadequate = ec.Inadequate,
                RiskId = ec.RiskId,
                CreatedAt = ec.CreatedAt,
                UpdatedAt = ec.UpdatedAt,
            })
            .ToListAsync();
    }

    public async Task<PaginationResult<ExitControlModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid riskId, Page? page)
    {
        ExitControlValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching exit controls for user {UserId} with term: {SearchTerm} and risk: {RiskId}",
            userId, searchTerm ?? "(none)", riskId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, riskId, page);

        var cachedResult = await GetFromCacheAsync<ExitControlModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(ExitControlConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, riskId, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ExitControlModel>> SearchAsync(string? name, Page page) 
    { 
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching exit controls with name filter: {ExitControlName}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<ExitControlModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(ExitControlConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<ExitControlModel>(cacheKey, countCacheKey, page, name);
    }
}