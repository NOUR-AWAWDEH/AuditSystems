using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Domain.Entities.Recommendations;

namespace AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;

public interface IRecommendationSearch
{
    Expression<Func<Recommendation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid observationId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid observationId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? observationId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<RecommendationModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid observationId, Page page, string cacheKey, string countCacheKey);
}