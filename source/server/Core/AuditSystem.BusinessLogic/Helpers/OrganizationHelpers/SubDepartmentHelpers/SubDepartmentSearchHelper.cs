using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

public class SubDepartmentSearchHelper : ISubDepartmentSearch
{
    private readonly IRepository<Guid, SubDepartment> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public SubDepartmentSearchHelper(
        IRepository<Guid, SubDepartment> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public Expression<Func<SubDepartment, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid departmentId)
    {
        return sl => sl.Department.SubLocation.Location.UserId == userId && 
                    (string.IsNullOrEmpty(searchTerm) || sl.Name.Contains(searchTerm)) &&
                    sl.DepartmentId == departmentId;
    }

    // Original method for backward compatibility
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, null, page);
    }

    // Method with required Guid departmentId (not nullable) - matches interface
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid departmentId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var departmentIdStr = departmentId.ToString();
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubDepartment, userId, normalizedSearchTerm, departmentIdStr, page.Number, page.Size);
        
        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubDepartment, userId, normalizedSearchTerm, departmentIdStr);
        
        return (cacheKey, countCacheKey);
    }

    // Updated method with nullable departmentId support (for internal use)
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? departmentId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var departmentIdStr = departmentId?.ToString() ?? "all";
        
        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey, 
            CacheKeys.SubDepartment, userId, normalizedSearchTerm, departmentIdStr, page.Number, page.Size);
        
        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.SubDepartment, userId, normalizedSearchTerm, departmentIdStr);
        
        return (cacheKey, countCacheKey);
    }

    // Original method for backward compatibility
    public async Task<PaginationResult<SubDepartmentModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        return await GetUserSearchFromRepositoryAsync(userId, searchTerm, null, page, cacheKey, countCacheKey);
    }

    // Updated method with departmentId support
    public async Task<PaginationResult<SubDepartmentModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid? departmentId, Page page, string cacheKey, string countCacheKey)
    {
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, departmentId ?? Guid.Empty));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<SubDepartmentModel>>(items);
        var result = new PaginationResult<SubDepartmentModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<SubDepartment> items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubDepartmentConstants.SubDepartmentTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubDepartmentConstants.SubDepartmentTags, CacheExpirations.MediumTerm);
    }
}