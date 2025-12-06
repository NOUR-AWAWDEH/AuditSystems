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
        await _validationHelper.ValidateJobTitleNameUniquenessAsync(jobTitleModel.Name, jobTitleModel.CompanyId);

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

    public async Task UpdateJobTitleAsync(JobTitleModel model, Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogInformation("Updating job title {JobTitleId} for user {UserId}",
            model.Id, userId);

        // Get the tracked entity from validation
        var existingEntity = await _validationHelper.ValidateJobTitleOwnershipAsync(model.Id, userId);

        // Validate that the new name doesn't conflict with another JobTitle in the same company
        await _validationHelper.ValidateJobTitleNameUniquenessAsync(model.Name, model.CompanyId, model.Id);

        // Map the model to the existing tracked entity
        mapper.Map(model, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existingEntity);

        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated job title {JobTitleId}", model.Id);
    }

    public async Task<IReadOnlyList<JobTitleModel>> GetJobTitlesByUserIdAsync(Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);

        _logger.LogDebug("Retrieving all job titles for user {UserId}", userId);

        var user = await _validationHelper.ValidateUserAsync(userId);
        return await repository.GetAll()
            .AsNoTracking()
            .Where(j => j.CompanyId == user.CompanyId)
            .Select(j => new JobTitleModel
            {
                Id = j.Id,
                Name = j.Name,
                CompanyId = j.CompanyId,
                CreatedAt = j.CreatedAt,
                UpdatedAt = j.UpdatedAt
            })
            .OrderBy(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginationResult<JobTitleModel>> SearchByUserIdAsync(string? searchTerm, Page page, Guid userId, Guid? companyId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        var user = await _validationHelper.ValidateUserAsync(userId);

        var targetCompanyId = companyId ?? user.CompanyId;

        _logger.LogDebug("Searching job titles for user {UserId} with term: {SearchTerm} and company: {CompanyId}",
            userId, searchTerm ?? "(none)", targetCompanyId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetCompanySearchCacheKeys(targetCompanyId, searchTerm, page);

        var cachedResult = await GetFromCacheAsync<JobTitleModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(JobTitleConstants.RetrievedJobTitleSearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetCompanySearchFromRepositoryAsync(targetCompanyId, searchTerm, page, cacheKey, countCacheKey);
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
