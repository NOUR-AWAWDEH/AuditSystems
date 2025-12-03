using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.JobTitleConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.JobTitleHelpers;

public class JobTitleSearchHelper(
    IRepository<Guid, JobTitle> repository,
    ICacheService cacheService,
    ILogger<JobTitleSearchHelper> logger) : IJobTitleSearch
{
    private readonly IRepository<Guid, JobTitle> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<JobTitleSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<JobTitle, bool>> BuildCompanySearchPredicate(Guid companyId, string? searchTerm)
    {
        return d => (string.IsNullOrEmpty(searchTerm) || d.Name.Contains(searchTerm)) && d.CompanyId == companyId;
    }

    public (string cacheKey, string countCacheKey) GetCompanySearchCacheKeys(Guid companyId, string? searchTerm, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";

        var cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.JobTitle,
            companyId,
            normalizedSearchTerm,
            page.Number,
            page.Size);

        var countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.JobTitle,
            companyId,
            normalizedSearchTerm);

        return (cacheKey, countCacheKey);
    }

    // Add the missing method required by interface
    public async Task<PaginationResult<JobTitleModel>> GetCompanySearchFromRepositoryAsync(
        Guid companyId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildCompanySearchPredicate(companyId, searchTerm);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(d => d.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(d => new JobTitleModel
            {
                Id = d.Id,
                Name = d.Name,
                CompanyId = d.CompanyId
            })
            .ToArrayAsync();

        var result = new PaginationResult<JobTitleModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };

        _ = CacheSearchResultsAsync(cacheKey, items)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.OnlyOnFaulted);
        _ = CacheSearchCountAsync(countCacheKey, totalCount)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.OnlyOnFaulted);

        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, JobTitleModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, JobTitleConstants.JobTitleTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, JobTitleConstants.JobTitleTags, CacheExpirations.MediumTerm);
    }
}