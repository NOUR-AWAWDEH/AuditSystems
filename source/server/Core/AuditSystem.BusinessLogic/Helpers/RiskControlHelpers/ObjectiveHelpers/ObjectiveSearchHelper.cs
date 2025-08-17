using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;

public class ObjectiveSearchHelper : IObjectiveSearch
{
    private readonly IRepository<Guid, Objective> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public ObjectiveSearchHelper(
        IRepository<Guid, Objective> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<Objective, bool>> BuildUserSearchPredicate(
        Guid userId, string? searchTerm, Guid riskControlMatrixId)
    {
        return ob => ob.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId == userId &&
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
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, riskControlMatrixIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.RiskControlMatrix, userId, normalizedSearchTerm, riskControlMatrixIdStr);

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
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, riskControlMatrixId));
         
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<ObjectiveModel>>(items);
        var result = new PaginationResult<ObjectiveModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<Objective> items)
    {
        await _cacheService.SetAsync(cacheKey, items, ObjectiveConstants.ObjectiveTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, ObjectiveConstants.ObjectiveTags, CacheExpirations.MediumTerm);
    }
}