using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Application.Constants.Processes;
using Microsoft.EntityFrameworkCore;
using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditProcessService> logger,
    IAuditProcessValidation validationHelper,
    IAuditProcessSearch searchHelper) : 
    UnifiedCacheService<Guid, AuditProcess, AuditProcessModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditProcess,
        AuditProcessConstants.AuditProcessTags,
        AuditProcessConstants.ListTags
    ),
    IAuditProcessService
{
    private readonly IAuditProcessValidation _validationHelper = validationHelper;
    private readonly IAuditProcessSearch _searchHelper = searchHelper;
    
    public async Task<Guid> CreateAuditProcessAsync(AuditProcessModel processModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(processModel, nameof(processModel));
        AuditProcessValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Creating audit process {ProcessName} for user {UserId}", processModel.ProcessName, userId);

        await _validationHelper.ValidateSubDepartmentOwnerShipAsync(processModel.SubDepartmentId, userId);

        var entity = mapper.Map<AuditProcess>(processModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        logger.LogInformation("Successfully created audit process {ProcessId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id, Guid userId)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving audit process {ProcessId} for user {UserId}", id, userId);
        
        var cachedResult = await TryGetFromCacheWithValidationAsync<AuditProcessModel>(
            id,
            userId,
            entity => entity.SubDepartment.Department.SubLocation.Location.UserId,
            AuditProcessConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            logger.LogDebug("Retrieved audit process {ProcessId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateAuditProcessOwnerShipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<AuditProcessModel>(entity);
    }

    public async Task DeleteAuditProcessAsync(Guid processId, Guid userId)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);
        
        logger.LogInformation("Deleting audit process {ProcessId} for user {UserId}", processId, userId);

        await _validationHelper.ValidateAuditProcessOwnerShipAsync(processId, userId);
        
        await repository.DeleteAsync(processId);

        await ClearEntityCacheAsync(processId);
        
        await ClearUserRelatedCachesAsync(userId, "AuditProcess");

        logger.LogInformation("Successfully deleted audit process {ProcessId}", processId);
    }
    
    public async Task UpdateAuditProcessAsync(AuditProcessModel processModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(processModel, nameof(processModel));
        AuditProcessValidationHelper.ValidateUserId(userId);

        logger.LogInformation("Updating audit process {ProcessId} for user {UserId}", processModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateAuditProcessOwnerShipAsync(processModel.Id, userId);
        
        mapper.Map(processModel, existingEntity);
        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        logger.LogInformation("Successfully updated audit process {ProcessId}", processModel.Id);
    }
    
    public async Task<ICollection<AuditProcessModel>> GetAuditProcessesByUserIdAsync(Guid userId)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);

        logger.LogDebug("Retrieving all audit processes for user {UserId}", userId);

        var query = repository.GetAll()
            .Include(ap => ap.SubDepartment)
            .ThenInclude(sd => sd.Department)
            .ThenInclude(d => d.SubLocation)
            .ThenInclude(sl => sl.Location)
            .Where(ap => ap.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(ap => new AuditProcessModel
            {
                Id = ap.Id,
                ProcessName = ap.ProcessName,
                SubDepartmentId = ap.SubDepartmentId,
            });
        
        var processes = await query.ToListAsync();
        logger.LogDebug("Retrieved {Count} audit processes for user {UserId}", processes.Count, userId);

        return processes;
    }

    public async Task<PaginationResult<AuditProcessModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid subDepartmentId, Page? page)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching audit processes for user {UserId} with term: {SearchTerm} and subdepartment: {SubDepartmentId}",
             userId, searchTerm ?? "(none)", subDepartmentId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, subDepartmentId, page);

        var cachedResult = await GetFromCacheAsync<AuditProcessModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(AuditProcessConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, subDepartmentId, page, cacheKey, countCacheKey);
    }
    
    public async Task<PaginationResult<AuditProcessModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        logger.LogDebug("Searching audit processes with name filter: {ProcessName}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<AuditProcessModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            logger.LogDebug(AuditProcessConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<AuditProcessModel>(cacheKey, countCacheKey, page, name);
    }
}