using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface IRiskSearch
{
    Expression<Func<Risk, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid observationId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid observationId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? observationId, Page page);
    Task<PaginationResult<RiskModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid observationId, Page page, string cacheKey, string countCacheKey);
}