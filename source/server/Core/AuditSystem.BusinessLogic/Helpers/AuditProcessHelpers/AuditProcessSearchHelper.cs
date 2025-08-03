using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;

public class AuditProcessSearchHelper : IAuditProcessSearch
{
    private readonly IRepository<Guid, AuditProcess> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public AuditProcessSearchHelper(
        IRepository<Guid, AuditProcess> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<AuditProcess, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid subDepartmentId)
    {
        return ap => ap.SubDepartment.Department.SubLocation.Location.UserId == userId &&  
                (string.IsNullOrEmpty(searchTerm) || ap.ProcessName.Contains(searchTerm)) &&
                ap.SubDepartmentId == subDepartmentId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid subDepartmentId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid subDepartmentId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subDepartmentIdStr = subDepartmentId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, subDepartmentIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, subDepartmentIdStr);

        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable subDepartmentId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subDepartmentId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subDepartmentIdStr = subDepartmentId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, subDepartmentIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, subDepartmentIdStr);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<AuditProcessModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid subDepartmentId, Page page, string cacheKey, string countCacheKey)
    {
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, subDepartmentId));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<AuditProcessModel>>(items);
        var result = new PaginationResult<AuditProcessModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<AuditProcess> items)
    {
        await _cacheService.SetAsync(cacheKey, items, AuditProcessConstants.AuditProcessTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, AuditProcessConstants.AuditProcessTags, CacheExpirations.MediumTerm);
    }
}