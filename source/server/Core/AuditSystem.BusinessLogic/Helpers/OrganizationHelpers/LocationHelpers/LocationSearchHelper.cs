using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;

public class LocationSearchHelper(
    IRepository<Guid, Location> repository,
    ILogger<LocationSearchHelper> logger,
    ICacheService cacheService
        ) : ILocationSearch
{
    private readonly IRepository<Guid, Location> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<LocationSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<Location, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm)
    {
        return l => l.CreatedByUserId == userId && 
                   (string.IsNullOrEmpty(searchTerm) || l.Name.Contains(searchTerm));
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Location, userId, normalizedSearchTerm, "all", page.Number, page.Size);
        
        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Location, userId, normalizedSearchTerm, "all");
        
        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<LocationModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();
    
        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderBy(l => l.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(l => new LocationModel
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address,
                CompanyId = l.CompanyId,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            })
            .ToArrayAsync();
       
        var result = new PaginationResult<LocationModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, LocationModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, LocationConstants.LocationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, LocationConstants.LocationTags, CacheExpirations.MediumTerm);
    }
}