using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.ControlDesignConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AuditSystem.BusinessLogic.Helpers.ControlDesignHelpers;

public class ControlDesignSearchHelper(
    IRepository<Guid, ControlDesign> repository,
    ICacheService cacheService,
    ILogger<ControlDesignSearchHelper> logger)
    : IControlDesignSearch
{
    private readonly IRepository<Guid, ControlDesign> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<ControlDesignSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


    public Expression<Func<ControlDesign, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid? exitControlId)
    {
        return cd => cd.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId == userId &&
                        (string.IsNullOrEmpty(searchTerm) || cd.Description.Contains(searchTerm)) &&
                        cd.ExitControlId == (exitControlId ?? cd.ExitControlId);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid exitControlId, Page page)
    {
        string normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var exitControlIdStr = exitControlId == Guid.Empty ? "all" : exitControlId.ToString();

        string cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.ControlDesign,
            userId,
            normalizedSearchTerm,
            exitControlIdStr,
            page.Number,
            page.Size
        );

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.ControlDesign,
            userId,
            normalizedSearchTerm,
            exitControlIdStr
        );

        return (cacheKey, countCacheKey);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? exitControlId, Page page)
    {
        string normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var exitControlIdStr = exitControlId?.ToString() ?? "all";

        string cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.ControlDesign,
            userId,
            normalizedSearchTerm,
            exitControlIdStr,
            page.Number,
            page.Size
        );

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.ControlDesign,
            userId,
            normalizedSearchTerm,
            exitControlIdStr
        );

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ControlDesignModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid exitControlId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, exitControlId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ap => new ControlDesignModel
            {
                Id = ap.Id,
                Description = ap.Description,
                ExitControlId = ap.ExitControlId,
                Inadequate = ap.Inadequate,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt
            })
            .OrderBy(cd => cd.CreatedAt)
            .ToArrayAsync();

        var result = new PaginationResult<ControlDesignModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };

        try
        {
            await Task.WhenAll(
                CacheSearchResultsAsync(cacheKey, items),
                CacheSearchCountAsync(countCacheKey, totalCount)
            );
            _logger.LogDebug("Successfully cached control design search results");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache control design search results");
        }

        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, ControlDesignModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, ControlDesignConstants.ControlDesignTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ControlDesignConstants.ControlDesignTags, CacheExpirations.MediumTerm);
    }
}