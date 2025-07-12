using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;


public interface IDepartmentSearch
{
    Expression<Func<Department, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid? subLocationId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subLocationId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<DepartmentModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid? subLocationId, Page page, string cacheKey, string countCacheKey);
}
