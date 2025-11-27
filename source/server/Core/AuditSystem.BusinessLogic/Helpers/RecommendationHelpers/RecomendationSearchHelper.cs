using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Domain.Entities.Recommendations;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Application.Constants.RecommendationConst;


namespace AuditSystem.BusinessLogic.Helpers.RecommendationHelpers;

public class RecommendationSearchHelper(
    IRepository<Guid, Recommendation> repository,
    ICacheService cacheService) : IRecommendationSearch
{
    private readonly IRepository<Guid, Recommendation> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

    public Expression<Func<Recommendation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid observationId)
    {
        return ap => ap.CreatedByUserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || ap.Description.Contains(searchTerm));
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid observationId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid observationId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var observationIdStr = observationId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Recommendation, userId, normalizedSearchTerm, observationIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Recommendation, userId, normalizedSearchTerm, observationIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable observationId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? observationId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var observationIdStr = observationId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Recommendation, userId, normalizedSearchTerm, observationIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Recommendation, userId, normalizedSearchTerm, observationIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RecommendationModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid observationId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, observationId);   
        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();
    
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ap => new RecommendationModel
            {
                Id = ap.Id,
                Title = ap.Title,
                Description = ap.Description,
                ObservationId = ap.ObservationId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt,
            })
            .OrderBy(o => o.CreatedAt)
            .ToArrayAsync();
    
        var result = new PaginationResult<RecommendationModel>
        {
            Items = items,
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

    private async Task CacheSearchResultsAsync(string cacheKey, RecommendationModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, RecommendationConstants.RecommendationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, RecommendationConstants.RecommendationTags, CacheExpirations.MediumTerm);
    }
}