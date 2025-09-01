using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;

public class ExitControlSearchHelper : IExitControlSearch
{
    private readonly IRepository<Guid, ExitControl> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ExitControlSearchHelper> _logger;

    public ExitControlSearchHelper(
        IRepository<Guid, ExitControl> repository,
        IMapper mapper,
        ILogger<ExitControlSearchHelper> logger,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Expression<Func<ExitControl, bool>> BuildUserSearchPredicate(    
        Guid userId, string? searchTerm, Guid riskId)
    {
        return r => r.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || r.Description.Contains(searchTerm)) &&
                r.RiskId == riskId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid riskId - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid riskId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var riskIdStr = riskId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, riskIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, riskIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable riskId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? riskId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var riskIdStr = riskId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, riskIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.ExitControl, userId, normalizedSearchTerm, riskIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<ExitControlModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid riskId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, riskId);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(ec => ec.CreatedAt).ThenBy(ec => ec.Id)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ec => new ExitControlModel
            {
                Id = ec.Id,
                Description = ec.Description,
                Inadequate = ec.Inadequate,
                RiskId = ec.RiskId,
                CreatedAt = ec.CreatedAt,
                UpdatedAt = ec.UpdatedAt
            })
            .ToListAsync();

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

    private async Task CacheSearchResultsAsync(string cacheKey, List<ExitControlModel> items)
    {
        await _cacheService.SetAsync(cacheKey, items, ExitControlConstants.ExitControlTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ExitControlConstants.ExitControlTags, CacheExpirations.MediumTerm);
    }
}