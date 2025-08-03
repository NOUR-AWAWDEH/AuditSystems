using System.Linq.Expressions;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;

public interface ISubProcessSearch
{
    Expression<Func<SubProcess, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid auditProcessId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid auditProcessId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? auditProcessId, Page page);
    Task<PaginationResult<SubProcessModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid auditProcessId, Page page, string cacheKey, string countCacheKey);
}