using System.Linq.Expressions;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Processes;


namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;


public interface IAuditProcessSearch
{
    Expression<Func<AuditProcess, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid subDepartmentId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid subDepartmentId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subDepartmentId, Page page);
    Task<PaginationResult<AuditProcessModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid subDepartmentId, Page page, string cacheKey, string countCacheKey);
}