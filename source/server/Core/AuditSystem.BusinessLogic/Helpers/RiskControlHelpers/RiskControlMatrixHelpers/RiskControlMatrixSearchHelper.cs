using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;

public class RiskControlMatrixSearchHelper : IRiskControlMatrixSearch
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public RiskControlMatrixSearchHelper(
        IRepository<Guid, RiskControlMatrix> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<RiskControlMatrix, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid subProcessId)
    {
        return sp => sp.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId &&
                (string.IsNullOrEmpty(searchTerm) || sp.Description.Contains(searchTerm)) &&
                sp.SubProcessId == subProcessId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid subProcessId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid subProcessId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subProcessIdStr = subProcessId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, subProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, subProcessIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable subProcessId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId, string? searchTerm, Guid? subProcessId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subProcessIdStr = subProcessId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, subProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubProcess, userId, normalizedSearchTerm, subProcessIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<RiskControlMatrixModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid subProcessId, Page page, string cacheKey, string countCacheKey)
    {
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, subProcessId));
         
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<RiskControlMatrixModel>>(items);
        var result = new PaginationResult<RiskControlMatrixModel>
        {
            Items = mappedItems,
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<RiskControlMatrix> items)
    {
        await _cacheService.SetAsync(cacheKey, items, RiskControlMatrixConstants.RiskControlMatrixTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, RiskControlMatrixConstants.RiskControlMatrixTags, CacheExpirations.MediumTerm);
    }
}