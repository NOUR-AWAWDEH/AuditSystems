using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentSearchHelper : IDepartmentSearch
{
    private readonly IRepository<Guid, Department> _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<DepartmentSearchHelper> _logger;

    public DepartmentSearchHelper(
        IRepository<Guid, Department> repository,
        ICacheService cacheService,
        ILogger<DepartmentSearchHelper> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Expression<Func<Department, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid subLocationId)
    {
        return d => d.SubLocation.Location.UserId == userId && 
                (string.IsNullOrEmpty(searchTerm) || d.Name.Contains(searchTerm)) &&
                d.SubLocationId == subLocationId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid subLocationId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid subLocationId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subLocationIdStr = subLocationId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Department, userId, normalizedSearchTerm, subLocationIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Department, userId, normalizedSearchTerm, subLocationIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable subLocationId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subLocationId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subLocationIdStr = subLocationId.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Department, userId, normalizedSearchTerm, subLocationIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Department, userId, normalizedSearchTerm, subLocationIdStr);

        return (cacheKey, countCacheKey);
    }

    // Add the missing method required by interface
    public async Task<PaginationResult<DepartmentModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid subLocationId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, subLocationId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();
        
        var items = await query
            .OrderBy(d => d.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(d => new DepartmentModel
            {
                Id = d.Id,
                Name = d.Name,
                SubLocationId = d.SubLocationId,
            })
            .ToArrayAsync();
    
        var result = new PaginationResult<DepartmentModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, DepartmentModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, DepartmentConstants.DepartmentTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, DepartmentConstants.DepartmentTags,CacheExpirations.MediumTerm);
    }
}