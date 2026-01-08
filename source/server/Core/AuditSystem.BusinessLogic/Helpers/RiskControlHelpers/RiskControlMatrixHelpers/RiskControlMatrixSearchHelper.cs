using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;

public class RiskControlMatrixSearchHelper(
    IRepository<Guid, RiskControlMatrix> repository,
    ILogger<RiskControlMatrixSearchHelper> logger,
    ICacheService cacheService) : IRiskControlMatrixSearch
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<RiskControlMatrixSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<RiskControlMatrix, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid auditProcessId)
    {
        return sp => sp.CreatedByUserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || sp.Description.Contains(searchTerm)) &&
                sp.AuditProcessId == auditProcessId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid auditProcessId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var auditProcessIdStr = auditProcessId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, auditProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, auditProcessIdStr);

        return (cacheKey, countCacheKey);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? auditProcessId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var auditProcessIdStr = auditProcessId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, auditProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, auditProcessIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid auditProcessId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, auditProcessId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(rcm => rcm.Description)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(rcm => new RiskControlMatrixModel
            {
                Id = rcm.Id,
                Description = rcm.Description,
                AuditProcessId = rcm.AuditProcessId,
                CreatedAt = rcm.CreatedAt,
                UpdatedAt = rcm.UpdatedAt
            })
            .ToArrayAsync();

        var result = new PaginationResult<RiskControlMatrixModel>
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


    private async Task CacheSearchResultsAsync(string cacheKey, RiskControlMatrixModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, RiskControlMatrixConstants.RiskControlMatrixTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, RiskControlMatrixConstants.RiskControlMatrixTags, CacheExpirations.MediumTerm);
    }
}