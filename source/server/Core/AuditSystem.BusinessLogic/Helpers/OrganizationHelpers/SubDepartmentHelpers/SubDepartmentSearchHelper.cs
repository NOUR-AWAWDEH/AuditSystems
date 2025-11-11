using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

public class SubDepartmentSearchHelper(
    IRepository<Guid, SubDepartment> repository,
    ILogger<SubDepartmentSearchHelper> logger,
    ICacheService cacheService) : ISubDepartmentSearch
{
    private readonly IRepository<Guid, SubDepartment> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<SubDepartmentSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<SubDepartment, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid departmentId)
    {
        return sl => sl.Department.Location.UserId == userId && 
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

    public async Task<PaginationResult<SubDepartmentModel>> GetUserSearchFromRepositoryAsync(
    Guid userId, string? searchTerm, Guid? departmentId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, departmentId ?? Guid.Empty);

        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(sd => sd.Name)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(sd => new SubDepartmentModel
            {
                Id = sd.Id,
                Name = sd.Name,
                DepartmentId = sd.DepartmentId,
                CreatedAt = sd.CreatedAt,
                UpdatedAt = sd.UpdatedAt
            })
            .ToArrayAsync();

        var result = new PaginationResult<SubDepartmentModel>
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


    private async Task CacheSearchResultsAsync(string cacheKey, SubDepartmentModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, SubDepartmentConstants.SubDepartmentTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SubDepartmentConstants.SubDepartmentTags, CacheExpirations.MediumTerm);
    }
}