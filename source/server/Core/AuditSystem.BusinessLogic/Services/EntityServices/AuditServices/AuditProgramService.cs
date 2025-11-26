using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditProgramService(
    IRepository<Guid, AuditProgram> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditProgramService> _logger,
    IAuditProgramValidation validationHelper,
    IAuditProgramSearch searchHelper) : 
    UnifiedCacheService<Guid, AuditProgram, AuditProgramModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.AuditProgram,
        AuditProgramConstants.AuditProgramTags,
        AuditProgramConstants.ListTags
    ),
    IAuditProgramService
{
    private readonly IAuditProgramValidation _validationHelper = validationHelper;
    private readonly IAuditProgramSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(auditProgramModel, nameof(auditProgramModel));
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Creating audit program {AuditProgramName} for user {UserId}", auditProgramModel.Description, userId);

        await _validationHelper.ValidateExitControlOwnershipAsync(auditProgramModel.ExitControlId, userId);

        var entity = mapper.Map<AuditProgram>(auditProgramModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity); 
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully created audit program {AuditProgramId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task DeleteAuditProgramAsync(Guid auditProgramId, Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);
            
        _logger.LogInformation("Deleting audit program {AuditProgramId} for user {UserId}", auditProgramId, userId);

        await _validationHelper.ValidateAuditProgramOwnershipAsync(auditProgramId, userId);

        var auditProgram = await repository.GetByIdAsync(auditProgramId);
        var exitControlId = auditProgram?.ExitControlId;

        await repository.DeleteAsync(auditProgramId);
        await ClearEntityCacheAsync(auditProgramId);
        
        // Clear all related caches
        await ClearUserRelatedCachesAsync(userId, AuditProgramConstants.EntityName, exitControlId);
        await InvalidateEntityTagsCacheAsync();
        
        _logger.LogInformation("Successfully deleted audit program {AuditProgramId}", auditProgramId);
    }

    public async Task<AuditProgramModel> GetAuditProgramByIdAsync(Guid id, Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving audit program {AuditProgramId} for user {UserId}", id, userId);

        // Try to get from cache first
        var cachedResult = await TryGetFromCacheWithValidationAsync<AuditProgramModel>(
            id, userId, AuditProgramConstants.AuditProgramOwnerIdSelector, 
            AuditProgramConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved audit program {AuditProgramId} from cache", id);
            return cachedResult;
        }

        // If not in cache, load from database with PROPER includes
        var entity = await repository.GetAll()
            .Include(ap => ap.ExitControl)
                .ThenInclude(ec => ec.Objective)
                .ThenInclude(o => o.RiskControlMatrix)
                .ThenInclude(rcm => rcm.SubProcess)
                .ThenInclude(sp => sp.AuditProcess)
                .ThenInclude(ap => ap.SubDepartment)
                .ThenInclude(sd => sd.Department)
                .ThenInclude(d => d.Location)
            .AsSplitQuery() // Important for deep includes
            .FirstOrDefaultAsync(ap => ap.Id == id);

        if (entity == null)
        {
            _logger.LogWarning("Audit program with ID {AuditProgramId} not found", id);
            throw new KeyNotFoundException($"Audit program with ID '{id}' was not found.");
        }

        // Validate ownership with safe method
        var ownerId = GetOwnerIdSafely(entity, id);
        if (ownerId == Guid.Empty)
        {
            _logger.LogError("Unable to determine owner for audit program {AuditProgramId}", id);
            throw new InvalidOperationException($"Unable to determine ownership for audit program {id}");
        }

        if (ownerId != userId)
        {
            _logger.LogWarning("User {UserId} not authorized to access audit program {AuditProgramId} owned by {OwnerId}", 
                userId, id, ownerId);
            throw new UnauthorizedAccessException(
                string.Format(AuditProgramConstants.UnauthorizedAccessMessage, userId, id));
        }

        // Cache the entity
        await CacheEntityAsync(entity);

        var result = mapper.Map<AuditProgramModel>(entity);
        _logger.LogDebug("Retrieved audit program {AuditProgramId} from repository and cached it", id);

        return result;
    }

    private Guid GetOwnerIdSafely(AuditProgram auditProgram, Guid auditProgramId)
    {
        try
        {
            // Step-by-step navigation with null checking
            var exitControl = auditProgram?.ExitControl;
            if (exitControl == null)
            {
                _logger.LogError("ExitControl is null for audit program {AuditProgramId}", auditProgramId);
                return Guid.Empty;
            }

            var objective = exitControl.Objective;
            if (objective == null)
            {
                _logger.LogError("Objective is null for exit control {ExitControlId}", exitControl.Id);
                return Guid.Empty;
            }

            var riskControlMatrix = objective.RiskControlMatrix;
            if (riskControlMatrix == null)
            {
                _logger.LogError("RiskControlMatrix is null for objective {ObjectiveId}", objective.Id);
                return Guid.Empty;
            }

            var subProcess = riskControlMatrix.SubProcess;
            if (subProcess == null)
            {
                _logger.LogError("SubProcess is null for risk control matrix {RCMId}", riskControlMatrix.Id);
                return Guid.Empty;
            }

            var auditProcess = subProcess.AuditProcess;
            if (auditProcess == null)
            {
                _logger.LogError("AuditProcess is null for sub process {SubProcessId}", subProcess.Id);
                return Guid.Empty;
            }

            var subDepartment = auditProcess.SubDepartment;
            if (subDepartment == null)
            {
                _logger.LogError("SubDepartment is null for audit process {AuditProcessId}", auditProcess.Id);
                return Guid.Empty;
            }

            var department = subDepartment.Department;
            if (department == null)
            {
                _logger.LogError("Department is null for sub department {SubDepartmentId}", subDepartment.Id);
                return Guid.Empty;
            }

            var location = department.Location;
            if (location == null)
            {
                _logger.LogError("Location is null for department {DepartmentId}", department.Id);
                return Guid.Empty;
            }

            return location.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting owner ID for audit program {AuditProgramId}", auditProgramId);
            return Guid.Empty;
        }
    }

    public async Task<IReadOnlyList<AuditProgramModel>> GetAuditProgramsByUserIdAsync(Guid userId)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retriving all audit programs for user {UserId}", userId);


        return await repository.GetAll()
            .AsNoTracking()
            .Where(AuditProgramConstants.AuditProgramOwnerPredicate(userId))
            .Select(d => new AuditProgramModel
            {
                Id = d.Id,
                Description = d.Description,
                ExitControlId = d.ExitControlId,
                ActionOwnerId = d.ActionOwnerId,
                ControlDesignId = d.ControlDesignId,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
            })
            .OrderBy(ap => ap.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<AuditProgramModel>> SearchAsync(string? name, Page? page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searcing audit programs whith name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditProgramModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(AuditProgramConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<AuditProgramModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<AuditProgramModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid exitControlId, Page? page)
    {
        AuditProgramValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching audit programs for user {UserId} with term: {SearchTerm} and exit control: {ExitControlId}",
            userId, searchTerm ?? "(none)", exitControlId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, exitControlId, page);

        var cachedResult = await GetFromCacheAsync<AuditProgramModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(AuditProgramConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        var result = await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, exitControlId, page, cacheKey, countCacheKey);
        return result;
    }

    public async Task UpdateAuditProgramAsync(AuditProgramModel auditProgramModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(auditProgramModel, nameof(auditProgramModel));
        AuditProgramValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating audit program {AuditProgramId} for user {UserId}", auditProgramModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateAuditProgramOwnershipAsync(auditProgramModel.Id, userId);
        
        mapper.Map(auditProgramModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Successfully updated audit program {AuditProgramId}", auditProgramModel.Id);
    }
}