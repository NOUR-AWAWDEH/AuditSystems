using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.SupportingDocs;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;

public class SupportingDocSearchHelper(
    IRepository<Guid, SupportingDoc> repository,
    ILogger<SupportingDocSearchHelper> logger,
    ICacheService cacheService) : ISupportingDocSearch
{
    private readonly IRepository<Guid, SupportingDoc> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    private readonly ILogger<SupportingDocSearchHelper> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public static Expression<Func<SupportingDoc, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm)
    {
        return s => s.UserId == userId && 
                (string.IsNullOrEmpty(searchTerm) || s.FileName.Contains(searchTerm));
    }

    public (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page)
    {
        var normalizedSearchTerm = searchTerm?.Trim().ToLowerInvariant() ?? "all";

        var cacheKey = string.Format(CacheKeys.UserSearchCacheKey,
            CacheKeys.SupportingDoc, userId, normalizedSearchTerm, page.Number, page.Size);

        var countCacheKey = string.Format(CacheKeys.UserSearchCountCacheKey,
            CacheKeys.SupportingDoc, userId, normalizedSearchTerm);

        return (cacheKey, countCacheKey);
    }



    public async Task<PaginationResult<SupportingDocModel>> GetUserSearchFromRepositoryAsync(
        Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey)
    {
        var predicate = BuildUserSearchPredicate(userId, searchTerm);
        var query = _repository.GetAll()
            .Where(predicate)
            .AsNoTracking();
    
        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(s => s.FileName)
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .Select( s => new SupportingDocModel
            {
                Id = s.Id,
                UserId = s.UserId,
                FileName = s.FileName,
                FileSize = s.FileSize,
                DriveFileId = s.DriveFileId,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
            })
            .ToArrayAsync();
    
        var result = new PaginationResult<SupportingDocModel>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    
        _ = CacheSearchResultsAsync(cacheKey, items)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.NotOnFaulted);
        _ = CacheSearchCountAsync(countCacheKey, totalCount)
                .ContinueWith(t => _logger.LogError(t.Exception, "Cache failed"), TaskContinuationOptions.NotOnFaulted);
        
        return result;
    }

    private async Task CacheSearchResultsAsync(string cacheKey, SupportingDocModel[] items)
    {
        await _cacheService.SetAsync(cacheKey, items, SupportingDocsConstants.SupportingDocTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SupportingDocsConstants.SupportingDocTags, CacheExpirations.MediumTerm);
    }
}