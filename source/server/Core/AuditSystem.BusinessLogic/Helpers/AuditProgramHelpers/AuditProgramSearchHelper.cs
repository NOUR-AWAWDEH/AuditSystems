using System.Linq.Expressions;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Application.Constants;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Application.Constants.Audit.AuditProgramConst;

namespace AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;

public class AuditProgramSearchHelper(
    IRepository<Guid, AuditProgram> repository,
    ICacheService cacheService,
    ILogger<AuditProgramSearchHelper> logger) : IAuditProgramSearch
{
    private readonly IRepository<Guid, AuditProgram> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<AuditProgramSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<AuditProgram, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid exitControlId)
    {
        return ap => ap.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId == userId &&
                    (string.IsNullOrEmpty(searchTerm) || ap.Description.Contains(searchTerm)) &&
                    ap.ExitControlId == exitControlId;
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid exitControlId, Page page)
    {
        string normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var exitControlIdStr = exitControlId == Guid.Empty ? "all" : exitControlId.ToString();

        string cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.AuditProgram,  
            userId,                  
            normalizedSearchTerm,    
            exitControlIdStr,        
            page.Number,             
            page.Size                
        );

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.AuditProgram,  
            userId,                  
            normalizedSearchTerm,    
            exitControlIdStr         
        );

        return (cacheKey, countCacheKey);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, Guid.Empty, page);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? exitControlId, Page page)
    {
        string normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var exitControlIdStr = exitControlId?.ToString() ?? "all";

        string cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.AuditProgram,
            userId,
            normalizedSearchTerm,
            exitControlIdStr,
            page.Number,
            page.Size
        );

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.AuditProgram,
            userId,
            normalizedSearchTerm,
            exitControlIdStr
        );

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<AuditProgramModel>> GetUserSearchFromRepositoryAsync(
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
            .Select(ap => new AuditProgramModel
            {
                Id = ap.Id,
                Description = ap.Description,
                ExitControlId = ap.ExitControlId,
                ControlDesignId = ap.ControlDesignId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt
            })
            .OrderBy(ap => ap.CreatedAt)
            .ToArrayAsync();

        var result = new PaginationResult<AuditProgramModel>
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
            _logger.LogDebug("Successfully cached audit program search results");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache audit program search results");
        }

        return result;
    }
    
    private async Task CacheSearchResultsAsync(string cacheKey, AuditProgramModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, AuditProgramConstants.AuditProgramTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, AuditProgramConstants.AuditProgramTags, CacheExpirations.MediumTerm);
    }
}