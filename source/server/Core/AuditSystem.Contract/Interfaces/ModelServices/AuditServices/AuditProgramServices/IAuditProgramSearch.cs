using System.Linq.Expressions;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;

public interface IAuditProgramSearch
{
    Expression<Func<AuditProgram, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid exitControlId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid exitControlId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? exitControlId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<AuditProgramModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid exitControlId, Page page, string cacheKey, string countCacheKey);
}
