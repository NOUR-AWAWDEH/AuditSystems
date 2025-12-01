using System.Linq.Expressions;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using Microsoft.Extensions.Logging;
using AuditSystem.Application.Constants;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Contract.Interfaces.Cache;

namespace AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;

public sealed class AuditProgramSearchHelper : IAuditProgramSearch
{
    private readonly IRepository<Guid, AuditProgram> _repository;
    private readonly ILogger<AuditProgramSearchHelper> _logger;

    public AuditProgramSearchHelper(
        IRepository<Guid, AuditProgram> repository,
        ILogger<AuditProgramSearchHelper> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // --------------------------------------------------------------------
    // ✅ Predicate
    // --------------------------------------------------------------------
    public Expression<Func<AuditProgram, bool>> BuildUserSearchPredicate(
        Guid userId,
        string? searchTerm,
        Guid exitControlId)
    {
        return ap =>
            ap.CreatedByUserId == userId &&
            (string.IsNullOrEmpty(searchTerm) || ap.Description.Contains(searchTerm)) &&
            ap.ExitControlId == exitControlId;
    }

    // --------------------------------------------------------------------
    // ✅ Cache Key Builders
    // --------------------------------------------------------------------
    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId,
        string? searchTerm,
        Guid exitControlId,
        Page page)
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
            page.Size);

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.AuditProgram,
            userId,
            normalizedSearchTerm,
            exitControlIdStr);

        return (cacheKey, countCacheKey);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId,
        string? searchTerm,
        Page page)
    {
        return GetUserSearchCacheKeys(userId, searchTerm, Guid.Empty, page);
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(
        Guid userId,
        string? searchTerm,
        Guid? exitControlId,
        Page page)
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
            page.Size);

        string countCacheKey = string.Format(
            CacheKeys.UserSearchCountCacheKey,
            CacheKeys.AuditProgram,
            userId,
            normalizedSearchTerm,
            exitControlIdStr);

        return (cacheKey, countCacheKey);
    }

    // --------------------------------------------------------------------
    // ✅ STANDARDIZED Repository Search (DTO ONLY)
    // --------------------------------------------------------------------
    public async Task<PaginationResult<AuditProgramModel>> GetUserSearchFromRepositoryAsync(
        Guid userId,
        string? searchTerm,
        Guid exitControlId,
        Page page,
        string cacheKey,
        string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm, exitControlId);

        var totalCount = await _repository.GetAll()
            .Where(predicate)
            .CountAsync();

        var items = await _repository.GetAll()
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(ap => ap.CreatedAt)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select(ap => new AuditProgramModel
            {
                Id = ap.Id,
                Description = ap.Description,
                ExitControlId = ap.ExitControlId,
                ActionOwnerId = ap.ActionOwnerId,
                ControlDesignId = ap.ControlDesignId,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt,
                ActionOwner = ap.ActionOwner == null ? null : new UserModel(
                    ap.ActionOwner.Id,
                    ap.ActionOwner.FirstName,
                    ap.ActionOwner.LastName,
                    ap.ActionOwner.Email,
                    Role.ActionOwner,
                    ap.ActionOwner.SubDepartment == null ? null : new SubDepartmentModel
                    {
                        Id = ap.ActionOwner.SubDepartment.Id,
                        Name = ap.ActionOwner.SubDepartment.Name,
                        DepartmentId = ap.ActionOwner.SubDepartment.DepartmentId
                    },
                    ap.ActionOwner.JobTitle == null ? null : new JobTitleModel(
                        ap.ActionOwner.JobTitle.Id,
                        ap.ActionOwner.JobTitle.Name,
                        ap.ActionOwner.JobTitle.CompanyId)
                )
            })
            .ToListAsync();

        var result = new PaginationResult<AuditProgramModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
        return result;
    }
}
