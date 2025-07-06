using System.Linq.Expressions;
using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.SupportingDocs;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;

public class SupportingDocSearchHelper
{
    private readonly IRepository<Guid, SupportingDoc> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public SupportingDocSearchHelper(
        IRepository<Guid, SupportingDoc> repository,
        IMapper mapper,
        ICacheService cacheService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

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
        var query = await _repository.GetAllAsync(BuildUserSearchPredicate(userId, searchTerm));
    
        var totalCount = query.Count;
        var items = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size)
            .ToList();
    
        var mappedItems = _mapper.Map<List<SupportingDocModel>>(items);
        var result = new PaginationResult<SupportingDocModel>
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

    private async Task CacheSearchResultsAsync(string cacheKey, List<SupportingDoc> items)
    {
        await _cacheService.SetAsync(cacheKey, items, SupportingDocsConstants.SupportingDocTags, CacheExpirations.MediumTerm);
    }

    private async Task CacheSearchCountAsync(string countCacheKey, int totalCount)
    {
        await _cacheService.SetAsync(countCacheKey, totalCount, SupportingDocsConstants.SupportingDocTags, CacheExpirations.MediumTerm);
    }
}