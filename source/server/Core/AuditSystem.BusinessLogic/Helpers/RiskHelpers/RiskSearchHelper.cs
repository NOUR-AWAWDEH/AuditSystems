using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.RiskHelpers;

public class RiskSearchHelper(
    IRepository<Guid, Risk> repository,
    ICacheService cacheService,
    ILogger<RiskSearchHelper> logger) : IRiskSearch
{
    private readonly IRepository<Guid, Risk> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<RiskSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<Risk, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid objectiveId)
    {
        return r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || r.Description.Contains(searchTerm)) &&
                r.ObjectiveId == objectiveId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid objectiveId - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid objectiveId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var objectiveIdStr = objectiveId == Guid.Empty ? "all" : objectiveId.ToString();

        var cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.Risk,
            userId,
            normalizedSearchTerm,
            objectiveIdStr,
            page.Number,
            page.Size
        );

        var countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Risk,
            userId,
            normalizedSearchTerm,
            objectiveIdStr
        );

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable objectiveId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? objectiveId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var objectiveIdStr = objectiveId?.ToString() ?? "all";
        
        var cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey, 
            CacheKeys.Objective,
            userId,
            normalizedSearchTerm,
            objectiveIdStr,
            page.Number,
            page.Size
        );

        var countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Objective,
            userId,
            normalizedSearchTerm,
            objectiveIdStr
        );

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid objectiveId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, objectiveId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy( r => r.Description)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(r => new RiskModel
            {
                Id = r.Id,
                Description = r.Description,
                ObjectiveId = r.ObjectiveId,
                RiskName = r.RiskName,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            })
            .ToArrayAsync();

        var result = new PaginationResult<RiskModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };

         // Proper async error handling
        try
        {
            await Task.WhenAll(
                CacheSearchResultsAsync(cacheKey, items),
                CacheSearchCountAsync(countCacheKey, totalCount)
            );
            _logger.LogDebug("Successfully cached risk search results");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache risk search results");
        }
        
        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, RiskModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, RiskConstants.RiskTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, RiskConstants.RiskTags, CacheExpirations.MediumTerm);
    }
}