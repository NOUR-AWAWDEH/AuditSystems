using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;

public class LocationSearchHelper : ILocationSearch
{
    private readonly IRepository<Guid, Location> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public LocationSearchHelper(
        IRepository<Guid, Location> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<Location, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm)
    {
        return l => l.UserId == userId && 
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
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<LocationModel>>(items);
        var result = new PaginationResult<LocationModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<Location> items)
    {
        await _cacheService.SetAsync(cacheKey, items, LocationConstants.LocationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, LocationConstants.LocationTags, CacheExpirations.MediumTerm);
    }
}