using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;

public interface IRiskControlMatrixSearch
{
    Expression<Func<RiskControlMatrix, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid subProcessId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid subProcessId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subProcessId, Page page);
    Task<PaginationResult<RiskControlMatrixModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid subProcessId, Page page, string cacheKey, string countCacheKey);
}