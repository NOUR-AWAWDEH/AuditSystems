using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.SubProcessHelpers;

public class SubProcessSearchHelper : ISubProcessSearch
{
    private readonly IRepository<Guid, SubProcess> _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<SubProcessSearchHelper> _logger;

    public SubProcessSearchHelper(
        IRepository<Guid, SubProcess> repository,
        ICacheService cacheService,
        ILogger<SubProcessSearchHelper> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Expression<Func<SubProcess, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid auditProcessId)
    {
        return sp => sp.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId &&  
                (string.IsNullOrEmpty(searchTerm) || sp.Particular.Contains(searchTerm)) &&
                sp.AuditProcessId == auditProcessId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid auditProcessId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid auditProcessId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var auditProcessIdStr = auditProcessId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, auditProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, auditProcessIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable auditProcessId support (for internal use)
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

    // Updated method with nullable auditProcessId support (for internal use)
    public async Task<PaginationResult<SubProcessModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid auditProcessId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, auditProcessId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(sp => sp.Particular)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(sp => new SubProcessModel
            {
                Id = sp.Id,
                Particular = sp.Particular,
                AuditProcessId = sp.AuditProcessId,
                CreatedAt = sp.CreatedAt,
                UpdatedAt = sp.UpdatedAt
            })
            .ToArrayAsync();

        var result = new PaginationResult<SubProcessModel>
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


    private async Task CacheSearchResultsAsync(string cacheKey, SubProcessModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubProcessConstants.SubProcessTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubProcessConstants.SubProcessTags, CacheExpirations.MediumTerm);
    }
}