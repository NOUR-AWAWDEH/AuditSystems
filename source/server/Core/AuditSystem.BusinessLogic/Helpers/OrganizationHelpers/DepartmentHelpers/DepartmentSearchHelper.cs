using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentSearchHelper : IDepartmentSearch
{
    private readonly IRepository<Guid, Department> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public DepartmentSearchHelper(
        IRepository<Guid, Department> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<Department, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid subLocationId)
    {
        return d => d.SubLocation.Location.UserId == userId && 
                (string.IsNullOrEmpty(searchTerm) || d.Name.Contains(searchTerm)) &&
                d.SubLocationId == subLocationId;  // Always filter by sublocation
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
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, subLocationId));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<DepartmentModel>>(items);
        var result = new PaginationResult<DepartmentModel>
        {
            Items = mappedItems,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    
        await Task.WhenAll(
            CacheSearchResultsAsync(cacheKey, items),
            CacheSearchCountAsync(countCacheKey, totalCount)
        );
    
        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, List<Department> items)
    {
        await _cacheService.SetAsync(cacheKey, items, DepartmentConstants.DepartmentTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, DepartmentConstants.DepartmentTags,CacheExpirations.MediumTerm);
    }
}