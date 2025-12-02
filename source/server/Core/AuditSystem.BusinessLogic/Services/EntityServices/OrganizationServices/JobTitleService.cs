using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
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

    public async Task<PaginationResult<JobTitleModel>> SearchByCompanyIdAsync(string? searchTerm, Guid companyId, Page page,Guid userId)
    {
        JobTitleValidationHelper.ValidateUserId(userId);
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching job titles for user {UserId} with term: {SearchTerm} and location: {LocationId}",
            userId, searchTerm ?? "(none)", companyId.ToString() ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetCompanySearchCacheKeys(companyId, searchTerm, page);

        var cachedResult = await GetFromCacheAsync<JobTitleModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(JobTitleConstants.RetrievedCompanySearchResultsFromCacheMessage);
            return cachedResult;
        }
        return await _searchHelper.GetCompanySearchFromRepositoryAsync(companyId, searchTerm, page, cacheKey, countCacheKey);
    }
}
