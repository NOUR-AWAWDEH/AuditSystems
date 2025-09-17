using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

public interface IExitControlSearch
{
    Expression<Func<ExitControl, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid riskId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid riskId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? riskId, Page page);
    Task<PaginationResult<ExitControlModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid riskId, Page page, string cacheKey, string countCacheKey);
}