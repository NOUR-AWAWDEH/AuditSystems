using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;

public class ObjectiveSearchHelper(
    IRepository<Guid, Objective> repository,
    ILogger<ObjectiveSearchHelper> logger,
    ICacheService cacheService) : IObjectiveSearch
{
    private readonly IRepository<Guid, Objective> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<ObjectiveSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<Objective, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid riskControlMatrixId)
    {
        return ob => ob.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || ob.Description.Contains(searchTerm)) &&
                ob.RiskControlMatrixId == riskControlMatrixId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid riskControlMatrixId - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid riskControlMatrixId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var riskControlMatrixIdStr = riskControlMatrixId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.Objective, userId, normalizedSearchTerm, riskControlMatrixIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.Objective, userId, normalizedSearchTerm, riskControlMatrixIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable riskControlMatrixId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? riskControlMatrixId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var riskControlMatrixIdStr = riskControlMatrixId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, riskControlMatrixIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, riskControlMatrixIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ObjectiveModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid riskControlMatrixId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, riskControlMatrixId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(o => o.Description)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(o => new ObjectiveModel
            {
                Id = o.Id,
                Description = o.Description,
                RiskControlMatrixId = o.RiskControlMatrixId,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            })
            .ToArrayAsync();

        var result = new PaginationResult<ObjectiveModel>
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


    private async Task CacheSearchResultsAsync(string cacheKey, ObjectiveModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, ObjectiveConstants.ObjectiveTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ObjectiveConstants.ObjectiveTags, CacheExpirations.MediumTerm);
    }
}