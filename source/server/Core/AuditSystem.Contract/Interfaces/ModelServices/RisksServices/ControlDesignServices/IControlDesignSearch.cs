using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

public interface IControlDesignSearch
{    
    Expression<Func<ControlDesign, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid? exitControlId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid exitControlId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? exitControlId, Page page);
    Task<PaginationResult<ControlDesignModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid exitControlId, Page page, string cacheKey, string countCacheKey);
}

