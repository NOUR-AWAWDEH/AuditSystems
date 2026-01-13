using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.CompanyConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.CompanyServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.CompanyHelpers;

public class CompanySearchHelper(
    IRepository<Guid, Company> repository,
    ICacheService cacheService,
    ILogger<CompanySearchHelper> logger) : ICompanySearch
{
    private readonly IRepository<Guid, Company> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<CompanySearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<Company, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm)
    {
        return c => c.CompanyUsers.Any(cu => cu.AppUserId == userId) &&
                (string.IsNullOrEmpty(searchTerm) || c.Name.Contains(searchTerm));
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";

        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey,
            CacheKeys.Company, userId, normalizedSearchTerm, "all", page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey,
            CacheKeys.Company, userId, normalizedSearchTerm, "all");

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<CompanyModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToArrayAsync();

        var result = new PaginationResult<CompanyModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };

        _ = CacheSearchResultsAsync(cacheKey, items)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.NotOnFaulted);
        _ = CacheSearchCountAsync(countCacheKey, totalCount)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.NotOnFaulted);

        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, CompanyModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, CompanyConstants.CompanyTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, CompanyConstants.CompanyTags, CacheExpirations.MediumTerm);
    }
}
