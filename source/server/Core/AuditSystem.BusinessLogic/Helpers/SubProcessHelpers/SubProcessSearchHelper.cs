using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.SubProcessHelpers;

public class SubProcessSearchHelper : ISubProcessSearch
{
    private readonly IRepository<Guid, SubProcess> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public SubProcessSearchHelper(
        IRepository<Guid, SubProcess> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
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
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, auditProcessIdStr, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey, 
            CacheKeys.AuditProcess, userId, normalizedSearchTerm, auditProcessIdStr);

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

    public async Task<PaginationResult<SubProcessModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid auditProcessId, Page page, string cacheKey, string countCacheKey)
    {
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm, auditProcessId));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<SubProcessModel>>(items);
        var result = new PaginationResult<SubProcessModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<SubProcess> items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubProcessConstants.SubProcessTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubProcessConstants.SubProcessTags, CacheExpirations.MediumTerm);
    }
}