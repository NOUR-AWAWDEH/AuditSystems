using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;

public class ExitControlSearchHelper(
    IRepository<Guid, ExitControl> repository,
    ILogger<ExitControlSearchHelper> logger,
    ICacheService cacheService) : IExitControlSearch
{
    private readonly IRepository<Guid, ExitControl> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<ExitControlSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<ExitControl, bool>> BuildUserSearchPredicate(    
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

    // Method with required Guid riskId - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid objectiveId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var objectiveIdStr = objectiveId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, objectiveIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, objectiveIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable riskId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? objectiveId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var objectiveIdStr = objectiveId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, objectiveIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, objectiveIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ExitControlModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid objectiveId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, objectiveId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(ec => ec.Description)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ec => new ExitControlModel
            {
                Id = ec.Id,
                Description = ec.Description,
                Inadequate = ec.Inadequate,
                ObjectiveId = ec.ObjectiveId,
                CreatedAt = ec.CreatedAt,
                UpdatedAt = ec.UpdatedAt
            })
            .ToArrayAsync();

        var result = new PaginationResult<ExitControlModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };

        // Cache async safely
        _ = CacheSearchResultsAsync(cacheKey, items)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.OnlyOnFaulted);
        _ = CacheSearchCountAsync(countCacheKey, totalCount)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache count failed"), TaskContinuationOptions.OnlyOnFaulted);

        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, ExitControlModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, ExitControlConstants.ExitControlTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ExitControlConstants.ExitControlTags, CacheExpirations.MediumTerm);
    }
}