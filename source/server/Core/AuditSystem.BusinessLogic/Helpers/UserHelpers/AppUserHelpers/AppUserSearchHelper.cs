using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.UserHelpers.AppUserHelpers;

public class AppUserSearchHelper(
    IRepository<Guid, AppUser> repository,
    ICacheService cacheService,
    ILogger<AppUserSearchHelper> logger) : IAppUserSearch
{
    private readonly IRepository<Guid, AppUser> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<AppUserSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Expression<Func<AppUser, bool>> BuildExpression(Guid userId, string? searchTerm, Guid? subDepartmentId)
    {
        return u => u.CreatedByUserId == userId &&
                    (string.IsNullOrEmpty(searchTerm) ||
                     u.FirstName.Contains(searchTerm) ||
                     u.LastName.Contains(searchTerm) ||
                     u.IdentityUser.UserName!.Contains(searchTerm) ||
                     u.IdentityUser.Email!.Contains(searchTerm)) &&
                    (subDepartmentId == null || u.SubDepartmentId == subDepartmentId);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subDepartmentId, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";
        var subDeptString = subDepartmentId?.ToString() ?? "all";

        var cacheKey = string.Format(
            CacheKeys.UserSearchCacheKey,
            CacheKeys.AppUser,
            userId,
            normalizedSearchTerm,
            page.Number,
            page.Size);

        var countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.AppUser,
            userId,
            normalizedSearchTerm);

        return (cacheKey, countCacheKey);
    }

    public async Task<PaginationResult<UserModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Guid? subDepartmentId, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildExpression(userId, searchTerm, subDepartmentId);

        var query = _repository.Query()
            .Include(u => u.IdentityUser)
            .Include(u => u.SubDepartment)
            .Include(u => u.JobTitle)
            .Where(predicate)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(u => new UserModel(
                u.Id,
                u.IdentityUser.UserName ?? string.Empty,
                u.FirstName,
                u.LastName,
                u.IdentityUser.Email ?? string.Empty,
                u.Role,
                u.SubDepartment == null ? null : new SubDepartmentModel
                {
                    Id = u.SubDepartment.Id,
                    Name = u.SubDepartment.Name,
                    DepartmentId = u.SubDepartment.DepartmentId
                },
                u.JobTitle == null ? null : new JobTitleModel
                {
                    Id = u.JobTitle.Id,
                    Name = u.JobTitle.Name,
                    CompanyId = u.JobTitle.CompanyId
                }))
            .ToArrayAsync();

        var result = new PaginationResult<UserModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, UserModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, new[] { "AppUser" }, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, new[] { "AppUser" }, CacheExpirations.MediumTerm);
    }
}
