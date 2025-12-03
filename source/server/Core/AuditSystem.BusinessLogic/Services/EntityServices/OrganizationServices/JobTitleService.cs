using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Application.Constants.Organization.JobTitleConst;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.JobTitleHelpers;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class JobTitleService(
    IRepository<Guid, JobTitle> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<JobTitleService> _logger,
    IJobTitleValidation validationHelper,
    IJobTitleSearch searchHelper) :
    UnifiedCacheService<Guid, JobTitle, JobTitleModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.JobTitle,
        JobTitleConstants.JobTitleTags,
        JobTitleConstants.ListTags
    ),
    IJobTitleService
{
    private readonly IJobTitleValidation _validationHelper = validationHelper;
    private readonly IJobTitleSearch _searchHelper = searchHelper;

    public async Task<Guid> CreateJobTitleAsync(JobTitleModel jobTitleModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(jobTitleModel, nameof(jobTitleModel));
        JobTitleValidationHelper.ValidateUserId(userId);

        await _validationHelper.ValidateJobTitleCreationAsync(userId, jobTitleModel.CompanyId);

        _logger.LogInformation("Creating job title for user {UserId}", userId);

        var entity = mapper.Map<JobTitle>(jobTitleModel);
        var createdEntity = await repository.CreateAsync(entity);

        await CacheEntityAsync(createdEntity);

        _logger.LogInformation("Successfully created job title {JobTitleId} for user {UserId}",
            createdEntity.Id, userId);

        return createdEntity.Id;
    }

    public async Task<JobTitleModel> GetJobTitleByIdAsync(Guid id, Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving job title {JobTitleId} for user {UserId}", id, userId);


        var cachedResult = await TryGetFromCacheWithValidationAsync<JobTitleModel>(
            id,
            userId,
            JobTitleConstants.JobTitleOwnerIdSelector,
            JobTitleConstants.UnauthorizedAccessMessage);

        if (cachedResult != null)
        {
            _logger.LogDebug("Retrieved job title {JobTitleId} from cache", id);
            return cachedResult;
        }

        var entity = await _validationHelper.ValidateJobTitleOwnershipAsync(id, userId);

        await CacheEntityAsync(entity);

        return mapper.Map<JobTitleModel>(entity);
    }

    public async Task DeleteJobTitleAsync(Guid jobTitleId, Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Deleting job title {JobTitleId} for user {UserId}", jobTitleId, userId);

        await _validationHelper.ValidateJobTitleOwnershipAsync(jobTitleId, userId);

        // Get the job title first to retrieve the CompanyId before deletion
        var jobTitle = await repository.GetByIdAsync(jobTitleId);
        Guid? companyId = jobTitle?.CompanyId;

        await repository.DeleteAsync(jobTitleId);
        await ClearEntityCacheAsync(jobTitleId);
        await ClearUserRelatedCachesAsync(userId, JobTitleConstants.EntityName, companyId);

        _logger.LogInformation("Successfully deleted job title {JobTitleId} for user {UserId}", jobTitleId, userId);
    }

    public async Task UpdateJobTitleAsync(Guid id, JobTitleModel model, Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating job title {JobTitleId} for user {UserId}",
            id, userId);

        // Get the tracked entity from validation
        var existingEntity = await _validationHelper.ValidateJobTitleOwnershipAsync(id, userId);

        // Map the model to the existing tracked entity
        mapper.Map(model, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated job title {JobTitleId}", id);
    }
   
    public async Task<IReadOnlyList<JobTitleModel>> GetJobTitlesByUserIdAsync(Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all job titles for user {UserId}", userId);

        return await repository.GetAll()
            .AsNoTracking()
            .Where(JobTitleConstants.JobTitleOwnerPredicate(userId))
            .Select(l => new JobTitleModel
            {
                Id = l.Id,
                Name = l.Name,
                CompanyId = l.CompanyId,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
            })
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<JobTitleModel>> SearchByCompanyIdAsync(string? searchTerm, Guid companyId, Page page, Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching job titles for user {UserId} with term: {SearchTerm} and location: {LocationId}",
            userId, searchTerm ?? "(none)", companyId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetCompanySearchCacheKeys(companyId, searchTerm, page);

        var cachedResult = await GetFromCacheAsync<JobTitleModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(JobTitleConstants.RetrievedJobTitleSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetCompanySearchFromRepositoryAsync(companyId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<JobTitleModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching job titles with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<JobTitleModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(JobTitleConstants.RetrievedJobTitleSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<JobTitleModel>(cacheKey, countCacheKey, page, name);
    }
}
