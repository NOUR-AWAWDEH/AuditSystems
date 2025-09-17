using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;

public interface IObjectiveSearch
{
    Expression<Func<Objective, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid riskControlMatrixId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid riskControlMatrixId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? riskControlMatrixId, Page page);
    Task<PaginationResult<ObjectiveModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid riskControlMatrixId, Page page, string cacheKey, string countCacheKey);
}