using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.Sublocation;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;

public class SubLocationSearchHelper
{
    private readonly IRepository<Guid, SubLocation> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public SubLocationSearchHelper(
        IRepository<Guid, SubLocation> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public static Expression<Func<SubLocation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm)
    {
        return sl => sl.Location.UserId == userId && 
                    (string.IsNullOrEmpty(searchTerm) || sl.Name.Contains(searchTerm));
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubLocation, userId, normalizedSearchTerm, page.Number, page.Size);
        
        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubLocation, userId, normalizedSearchTerm);
        
        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<SubLocationModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<SubLocationModel>>(items);
        var result = new PaginationResult<SubLocationModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<SubLocation> items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubLocationConstants.SubLocationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubLocationConstants.SubLocationTags, CacheExpirations.MediumTerm);
    }
}