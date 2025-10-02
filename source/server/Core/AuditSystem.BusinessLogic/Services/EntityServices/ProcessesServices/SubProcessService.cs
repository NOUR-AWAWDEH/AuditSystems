using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SubProcessService> _logger,
    ISubProcessValidation validationHelper,
    ISubProcessSearch searchHelper ) :
    UnifiedCacheService<Guid, SubProcess, SubProcessModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.SubProcess,
        SubProcessConstants.SubProcessTags,
        SubProcessConstants.ListTags
    ),
    ISubProcessService
{

    private readonly ISubProcessValidation _validationHelper = validationHelper;
    private readonly ISubProcessSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));
        AuditProcessValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Creating sub process {SubProcessId} for user {UserId}", subProcessModel.Id, userId);
        
        await _validationHelper.ValidateAuditProcessOwnershipAsync(subProcessModel.AuditProcessId, userId);

        var entity = mapper.Map<SubProcess>(subProcessModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created sub process {SubProcessId} for user {UserId}", createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<SubProcessModel> GetSubProcessByIdAsync(Guid id, Guid userId)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving sub process {SubProcessId} for user {UserId}", id, userId);

        var cachedResult = await TryGetFromCacheWithValidationAsync<SubProcessModel>(
            id,
            userId,
            entity => entity.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId,
            SubProcessConstants.UnauthorizedAccessMessage);
        
        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved sub process {SubProcessId} from cache for user {UserId}", id, userId);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateSubProcessOwnershipAsync(id, userId);
        await CacheEntityAsync(entity);
        return mapper.Map<SubProcessModel>(entity);
    }

    public async Task DeleteSubProcessAsync(Guid subProcessId, Guid userId)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Deleting sub process {SubProcessId} for user {UserId}", subProcessId, userId);

        await _validationHelper.ValidateSubProcessOwnershipAsync(subProcessId, userId);
        
        var subProcess = await repository.GetByIdAsync(subProcessId);
        Guid? auditProcessId = subProcess?.AuditProcessId;

        await repository.DeleteAsync(subProcessId);
        await ClearEntityCacheAsync(subProcessId);
        await ClearUserRelatedCachesAsync(userId, SubProcessConstants.EntityName, auditProcessId);

        _logger.LogInformation("Successfully deleted sub process {SubProcessId} for user {UserId}", subProcessId, userId);
    }
    
    public async Task UpdateSubProcessAsync(SubProcessModel subProcessModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));
        SubDepartmentValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating sub process {SubProcessId} for user {UserId}", subProcessModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateSubProcessOwnershipAsync(subProcessModel.Id, userId);
        await _validationHelper.ValidateAuditProcessOwnershipAsync(existingEntity.AuditProcessId, userId);

        mapper.Map(subProcessModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated sub process {SubProcessId} for user {UserId}", existingEntity.Id, userId);
    }

    public async Task<IReadOnlyList<SubProcessModel>> GetSubProcessesByUserIdAsync(Guid userId)
    {
        AuditProcessValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all sub processes for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(sp => sp.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId)
            .Select(sp => new SubProcessModel
            {
                Id = sp.Id,
                Particular = sp.Particular,
                AuditProcessId = sp.AuditProcessId,
                CreatedAt = sp.CreatedAt,
                UpdatedAt = sp.UpdatedAt,
            })
            .OrderBy(sp => sp.CreatedAt)
            .ToListAsync();

    }

    public async Task<PaginationResult<SubProcessModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid auditProcessId, Page? page)
    {
        SubDepartmentValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching sub processes for user {UserId} with term: {SearchTerm} and audit process: {AuditProcessId}",
                userId, searchTerm ?? "(none)", auditProcessId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, auditProcessId, page);

        var cachedResult = await GetFromCacheAsync<SubProcessModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(SubProcessConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, auditProcessId, page, cacheKey, countCacheKey);
    }
    
    public async Task<PaginationResult<SubProcessModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching sub processes  with name filter: {Particular}", name?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<SubProcessModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null) 
        {
            _logger.LogDebug(SubProcessConstants.RetrievedSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<SubProcessModel>(cacheKey, countCacheKey, page, name);
    }
}