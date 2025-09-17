using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;

public interface ISubDepartmentSearch
{
    Expression<Func<SubDepartment, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid departmentId );
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid departmentId, Page page);
    Task<PaginationResult<SubDepartmentModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
    Task<PaginationResult<SubDepartmentModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid? departmentId, Page page, string cacheKey, string countCacheKey);
}
