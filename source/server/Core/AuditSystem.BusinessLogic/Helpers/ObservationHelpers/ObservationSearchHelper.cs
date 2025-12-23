using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Observations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;


namespace AuditSystem.BusinessLogic.Helpers.ObservationHelpers;

public class ObservationSearchHelper(
    IRepository<Guid, Observation> repository,
    ICacheService cacheService) : IObservationSearch
{
    private readonly IRepository<Guid, Observation> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

    public Expression<Func<Observation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid auditProgramId)
    {
        return ap => ap.CreatedByUserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || ap.Highlight.Contains(searchTerm));
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid auditProgramId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid auditProgramId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var auditProgramIdStr = auditProgramId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Observation, userId, normalizedSearchTerm, auditProgramIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Observation, userId, normalizedSearchTerm, auditProgramIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable auditProgramId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? auditProgramId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var auditProgramIdStr = auditProgramId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Observation, userId, normalizedSearchTerm, auditProgramIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Observation, userId, normalizedSearchTerm, auditProgramIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ObservationModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid auditProgramId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, auditProgramId);   
        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();
    
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ap => new ObservationModel
            {
                Id = ap.Id,
                Background = ap.Background,
                Highlight = ap.Highlight,
                IsActive =  ap.IsActive,
                Note = ap.Note,
                AuditProgramId = ap.AuditProgramId,
                RateId =  ap.RateId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt,
            })
            .OrderBy(o => o.CreatedAt)
            .ToArrayAsync();
    
        var result = new PaginationResult<ObservationModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, ObservationModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, ObservationConstants.ObservationTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ObservationConstants.ObservationTags, CacheExpirations.MediumTerm);
    }
}