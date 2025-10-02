using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;

public class SubLocationSearchHelper(
    IRepository<Guid, SubLocation> repository,
    ILogger<SubLocationSearchHelper> logger,
    ICacheService cacheService) : ISubLocationSearch
{
    private readonly IRepository<Guid, SubLocation> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<SubLocationSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<SubLocation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid locationId)
    {
        return sl => sl.Location.UserId == userId && 
                    (string.IsNullOrEmpty(searchTerm) || sl.Name.Contains(searchTerm)) &&
                    sl.LocationId == locationId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, page);
    }

    // Method with required Guid locationId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid locationId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var locationIdStr = locationId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubLocation, userId, normalizedSearchTerm, locationIdStr, page.Number, page.Size);
        
        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubLocation, userId, normalizedSearchTerm, locationIdStr);
        
        return (cacheKey, countCacheKey);
    }

    // Original method for backward compatibility
    public async Task<PaginationResult<SubLocationModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        return await GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<SubLocationModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid locationId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, locationId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(sl => sl.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(sl => new SubLocationModel
            {
                Id = sl.Id,
                Name = sl.Name,
                LocationId = sl.LocationId,
                CreatedAt = sl.CreatedAt,
                UpdatedAt = sl.UpdatedAt
            })
            .AsNoTracking()
            .ToArrayAsync();

        var result = new PaginationResult<SubLocationModel>
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


    private async Task CacheSearchResultsAsync(string cacheKey, SubLocationModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubLocationConstants.SubLocationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubLocationConstants.SubLocationTags, CacheExpirations.MediumTerm);
    }
}